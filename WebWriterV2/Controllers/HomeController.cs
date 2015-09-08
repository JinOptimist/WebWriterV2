using System;
using System.Linq;
using System.Web.Mvc;
using Autofac;
using CopyVk;
using Dao.IRepository;
using Dao.Model;
using NLog;
using NLog.Common;
using NLog.Config;
using VkApi.Web;
using WebWriterV2.GetUserFromJsonFile;
using WebWriterV2.Models;
using WebWriterV2.SecondThread;
using WebWriterV2.Utility;

namespace WebWriterV2.Controllers
{
    public class HomeController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public IUserRepository UserRepository { get; set; }
        public IFriendIdRepository FriendIdRepository { get; set; }

        public HomeController()
        {
            using (var scope = StaticContainer.Container.BeginLifetimeScope())
            {
                UserRepository = scope.Resolve<IUserRepository>();
                FriendIdRepository = scope.Resolve<IFriendIdRepository>();
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SecondThread()
        {
            var model = CreateMarkViewModel();
            return View(model);
        }

        public ActionResult SecondThreadInfo()
        {
            var model = CreateMarkViewModel();
            return Json(new
            {
                state = model.TaskStatus, 
                currentUserName = model.CurrentVkUser.FirstName + " " + model.CurrentVkUser.LastName,
                currentUserId = model.CurrentVkUser.VkId,
                friends = model.Friends, 
                totalUser = model.TotalUserFromDb, 
                totalFriend = model.TotalFriendFromDb
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StartSecondThread(int? id)
        {
            var mark = Mark.Instance;
            mark.Start(id ?? 1);
            return RedirectToAction("SecondThread");
        }

        public ActionResult StopSecondThread()
        {
            var mark = Mark.Instance;
            mark.Stop();
            return RedirectToAction("SecondThread");
        }

        public ActionResult Count()
        {
            return View(UserRepository.CountUsers());
        }

        public ActionResult AllUser()
        {
            var users = UserRepository.GetAllUserFromVk();
            return View(users);
        }

        public ActionResult RemoveAllUser()
        {
            UserRepository.RemoveAllUserFromVk();
            return RedirectToAction("AllUser");
        }

        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(UserFromVk userFromVk)
        {
            var api = new VkApiCaller();
            var vkUser = api.GetVkUser(userFromVk.VkId);
            var friends = api.GetUserFriends(userFromVk.VkId);
            if (friends != null)
            {
                vkUser.FriendIds = friends.ToList();
            }
            var myUser = UserConverter.GetUserFromVk(vkUser);
            return View(myUser);
        }

        public ActionResult DownloadUserFromVk(long vkUserId)
        {
            if (vkUserId == default(long) || vkUserId < 1)
                return Json(new { isSuccessful = false, isAlreadyExists = false, content = "User not found" }, JsonRequestBehavior.AllowGet);

            if (UserRepository.ExistVkUser(vkUserId))
                return Json(new { isSuccessful = false, isAlreadyExists = true, content = "User already exists" }, JsonRequestBehavior.AllowGet);

            try
            {
                var myUser = Downloader.Download(vkUserId);
                UserRepository.SaveUserFromVk(myUser);
                return Json(new { isSuccessful = true, isAlreadyExists = false, content = myUser.FirstName + myUser.LastName }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Logger.Error("DownloadUserFromVk", e);

                return Json(new { isSuccessful = false, isAlreadyExists = false, content = "All very bad" }, JsonRequestBehavior.AllowGet);
            }
        }

        private MarkViewModel CreateMarkViewModel()
        {
            var mark = Mark.Instance;
            var model = new MarkViewModel
            {
                TaskStatus = mark.GetTaskStatus(),
                Friends = mark.Friends,
                CurrentVkUser = UserRepository.GetUserByVkId(mark.CurrentVkUserId),
                TotalFriendFromDb = FriendIdRepository.CountFriendId(),
                TotalUserFromDb = UserRepository.CountUsers()
            };

            return model;
        }
    }
}
