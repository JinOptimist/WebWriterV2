using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Dao.IRepository;
using Dao.Model;
using NLog;
using WebWriterV2.VkUtility;

namespace WebWriterV2.SecondThread
{
    public sealed class Mark
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly Mark mark = new Mark();

        private Task _task;

        private CancellationTokenSource cts;

        private readonly IUserRepository _userRepository;

        private Mark()
        {
            using (var scope = StaticContainer.Container.BeginLifetimeScope())
            {
                _userRepository = scope.Resolve<IUserRepository>();
            }

            CreateTask();
        }

        private void CreateTask()
        {
            cts = new CancellationTokenSource();
            _task = new Task(() => OneStep(cts.Token), cts.Token);
        }

        public static Mark Instance
        {
            get { return mark; }
        }

        public long Start()
        {
            if (_task.Status == TaskStatus.Running)
            {
                return CurrentVkUserId;
            }

            Stop();//Пытаемся остановить на всякий случай :)
            CreateTask();
            _task.Start();
            return CurrentVkUserId;
        }

        public void Stop()
        {
            try
            {
                cts.Cancel();
            }
            catch (Exception)
            {
                Logger.Info("I can't stop Second Thread");
            }
        }

        public TaskStatus GetTaskStatus()
        {
            return _task.Status;
        }

        public long CurrentVkUserId { get; private set; }

        public List<FriendWithState> Friends { get; private set; }

        private void OneStep(CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    CurrentVkUserId = _userRepository.GetUnsaveUserVkId();

                    Logger.Info("Start copy new user. Id {0}, time {1}", CurrentVkUserId, DateTime.Now.ToLongTimeString());
                    Friends = new List<FriendWithState>();
                    UserFromVk userFromVk = new UserFromVk();
                    try
                    {
                        userFromVk = Downloader.Download(CurrentVkUserId);
                    }
                    catch (Exception)
                    {
                        Logger.Error("OneStep. Can't download user from VK id {0}, time {1}", CurrentVkUserId, DateTime.Now.ToLongTimeString());
                    }

                    try
                    {
                        _userRepository.SaveUserFromVk(userFromVk);
                    }
                    catch (Exception)
                    {
                        Logger.Error("OneStep. Can't save user to my DB id {0}, time {1}", CurrentVkUserId, DateTime.Now.ToLongTimeString());
                    }

                    if (userFromVk != null && userFromVk.FriendIds != null)
                        foreach (var friendId in userFromVk.FriendIds)
                        {
                            var friendWithState = new FriendWithState
                            {
                                VkUserId = friendId.FriendsId,
                                State = FriendState.Wait
                            };
                            Friends.Add(friendWithState);
                        }

                    foreach (var friend in Friends)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        friend.State = DownloadUser(friend.VkUserId);
                    }

                    Logger.Info("Complete copy user. Id {0}, time {1}", CurrentVkUserId, DateTime.Now.ToLongTimeString());

                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            finally
            {
                Logger.Info("Second Thread was Cancel");
            }
        }

        private FriendState DownloadUser(long vkUserId)
        {
            if (vkUserId == default(long) || vkUserId < 1)
                return FriendState.Fail;

            if (_userRepository.ExistVkUser(vkUserId))
                return FriendState.AlreadyExist;

            try
            {
                var myUser = Downloader.Download(vkUserId);
                _userRepository.SaveUserFromVk(myUser);
                return FriendState.Done;
            }
            catch (Exception e)
            {
                Logger.Error("DownloadUser. Can't save or deownload. User Id-{0}. E.InnerException {1}", vkUserId, e.InnerException);
                return FriendState.Fail;
            }
        }
    }
}