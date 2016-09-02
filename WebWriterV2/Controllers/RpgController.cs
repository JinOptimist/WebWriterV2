using System;
using System.Linq;
using System.Web.Mvc;
using Autofac;
using Dao.IRepository;
using Newtonsoft.Json;
using NLog;
using WebWriterV2.Models.rpg;
using WebWriterV2.RpgUtility;
using WebWriterV2.VkUtility;

namespace WebWriterV2.Controllers
{
    public class RpgController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly JsonSerializerSettings JsonSettings
            = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };

        public IQuestRepository QuestRepository { get; set; }
        public IEventRepository EventRepository { get; set; }

        public RpgController()
        {
            using (var scope = StaticContainer.Container.BeginLifetimeScope())
            {
                QuestRepository = scope.Resolve<IQuestRepository>();
                EventRepository = scope.Resolve<IEventRepository>();
            }
        }

        public ActionResult RouteForAngular(string url)
        {
            return View("Index", (object)url);
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetListRace()
        {
            var listRace = Enum.GetValues(typeof(Race)).Cast<Race>();
            var listNameValue = listRace.Select(race => new
            {
                name = Enum.GetName(typeof(Race), race),
                value = race
            });

            return new JsonResult
            {
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(listNameValue),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetListSex()
        {
            var listSex = Enum.GetValues(typeof(Sex)).Cast<Sex>();
            var listNameValue = listSex.Select(sex => new
            {
                name = Enum.GetName(typeof(Sex), sex),
                value = sex
            });

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(listNameValue),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SaveQuest(string jsonQuest)
        {
            var quest = SerializeHelper.Deserialize<Quest>(jsonQuest);

            return new JsonResult
            {
                Data = true,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetHeroes()
        {
            return new JsonResult
            {
                Data = SerializeHelper.Serialize(GenerateData.GetHeroes()),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetRandomQuest()
        {
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(GenerateData.GetQuests().First(), JsonSettings),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetGuildInfo()
        {
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(GenerateData.GetGuild(), JsonSettings),
                //Data = SerializeHelper.Serialize(GenerateData.GetGuild()),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        public JsonResult Test(int id)
        {

            //var quest = GenerateData.GetQuests().FirstOrDefault();
            //QuestRepository.Save(quest);

            EventRepository.RemoveEventAndHisChildren(id);

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject("+"),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
