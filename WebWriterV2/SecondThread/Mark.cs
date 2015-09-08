using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Dao.IRepository;
using NLog;
using WebWriterV2.Utility;

namespace WebWriterV2.SecondThread
{
    public sealed class Mark
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly Mark mark = new Mark();

        private readonly Task _task;

        private IUserRepository userRepository;

        private Mark()
        {
            using (var scope = StaticContainer.Container.BeginLifetimeScope())
            {
                userRepository = scope.Resolve<IUserRepository>();
            }

            _task = new Task(OneStep);
        }

        public static Mark Instance
        {
            get { return mark; }
        }

        public long Start(long startVkId)
        {
            if (_task.Status == TaskStatus.Running)
            {
                return CurrentVkUserId;
            }

            CurrentVkUserId = startVkId;
            _task.Start();
            return CurrentVkUserId;
        }

        public TaskStatus GetTaskStatus()
        {
            return _task.Status;
        }

        public long CurrentVkUserId { get; private set; }

        /// <summary>
        /// Key = VkUserId
        /// </summary>
        public List<FriendWithState> Friends { get; private set; }

        private void OneStep()
        {
            Friends = new List<FriendWithState>();
            var userFromVk = Downloader.Download(CurrentVkUserId);
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
                friend.State = DownloadUser(friend.VkUserId);
            }

            foreach (var friend in userFromVk.FriendIds)
            {
                CurrentVkUserId = friend.FriendsId;
                OneStep();
            }
        }

        private FriendState DownloadUser(long vkUserId)
        {
            if (vkUserId == default(long) || vkUserId < 1)
                return FriendState.Fail;

            if (userRepository.ExistVkUser(vkUserId))
                return FriendState.AlreadyExist;

            try
            {
                var myUser = Downloader.Download(vkUserId);
                userRepository.SaveUserFromVk(myUser);
                return FriendState.Done;
            }
            catch (Exception e)
            {
                Logger.Error("DownloadUser", e);
                return FriendState.Fail;
            }
        }
    }
}