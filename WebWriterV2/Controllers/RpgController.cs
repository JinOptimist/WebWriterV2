using System;
using System.Linq;
using System.Web.Mvc;
using Autofac;
using Dao.IRepository;
using Dao.Model;
using Newtonsoft.Json;
using NLog;
//using WebWriterV2.Models.rpg;
using WebWriterV2.RpgUtility;
using WebWriterV2.VkUtility;
using Dao;
using WebWriterV2.FrontModels;

namespace WebWriterV2.Controllers
{
    public class RpgController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly JsonSerializerSettings JsonSettings
            = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };

        public IEventRepository EventRepository { get; set; }
        public IQuestRepository QuestRepository { get; set; }
        public IHeroRepository HeroRepository { get; set; }
        public ISkillRepository SkillRepository { get; set; }

        public RpgController()
        {
            using (var scope = StaticContainer.Container.BeginLifetimeScope())
            {
                EventRepository = scope.Resolve<IEventRepository>();
                QuestRepository = scope.Resolve<IQuestRepository>();
                HeroRepository = scope.Resolve<IHeroRepository>();
                SkillRepository = scope.Resolve<ISkillRepository>();
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
                Data = JsonConvert.SerializeObject(listNameValue),
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



        public JsonResult GetGuildInfo()
        {
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(GenerateData.GetGuild(), JsonSettings),
                //Data = SerializeHelper.Serialize(GenerateData.GetGuild()),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /* ************** Hero ************** */
        public JsonResult GetHeroes()
        {
            var heroes = HeroRepository.GetAll();
            var frontHeroes = heroes.Select(x => new FrontHero(x)).ToList();
            return new JsonResult
            {
                Data = SerializeHelper.Serialize(frontHeroes),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveAllHeroes()
        {
            var h = HeroRepository.GetAll();
            h.ForEach(HeroRepository.Remove);
            return new JsonResult
            {
                Data = true,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /* ************** Skill ************** */
        public JsonResult GetSkill(SkillSchool skillSchool)
        {
            var skillsFromDb = SkillRepository.GetBySchool(skillSchool);
            var frontSkill = skillsFromDb.Select(x => new FrontSkill(x));
            return new JsonResult
            {
                Data = SerializeHelper.Serialize(frontSkill),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetSkills()
        {
            var skillsFromDb = SkillRepository.GetAll();
            var frontSkill = skillsFromDb.Select(x => new FrontSkill(x));
            return new JsonResult
            {
                Data = SerializeHelper.Serialize(frontSkill),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveAllSkills()
        {
            var skillsFromDb = SkillRepository.GetAll();
            skillsFromDb.ForEach(SkillRepository.Remove);
            return new JsonResult
            {
                Data = SerializeHelper.Serialize(true),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /* ************** Quest ************** */
        public JsonResult GetQuest(long id)
        {
            var quest = QuestRepository.GetWithRootEvent(id);
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(quest, JsonSettings),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetQuests()
        {
            var quests = QuestRepository.GetAll();
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(quests, JsonSettings),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetOneQuest()
        {
            var quest = QuestRepository.GetAllWithRootEvent().FirstOrDefault();
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(quest, JsonSettings),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveQuest(long id)
        {
            var result = true;
            try
            {
                QuestRepository.Remove(id);
            }
            catch (Exception e)
            {
                result = false;
            }

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(result),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SaveQuest(string jsonQuest)
        {
            var quest = SerializeHelper.Deserialize<Quest>(jsonQuest);
            QuestRepository.Save(quest);

            return new JsonResult
            {
                Data = true,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /* ************** Event ************** */
        public JsonResult GetAllEvents(long questId)
        {
            var events = EventRepository.GetEventsWithChildren(questId);
            var frontEvents = events.Select(x => new FrontEvent(x)).ToList();
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(frontEvents),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetEventChildren(long id)
        {
            var eventFromDb = EventRepository.GetWithChildren(id);
            var frontEvents = new FrontEvent(eventFromDb);
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(frontEvents),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /* ************** Init Db ************** */
        public JsonResult Init()
        {
            var quests = QuestRepository.GetAll();
            if (quests.Count == 0)
            {
                var quest = GenerateData.GetQuest();
                QuestRepository.Save(quest);
            }

            var skills = SkillRepository.GetAll();
            if (skills.Count == 0)
            {
                SkillRepository.Save(GenerateData.GenerateSkills());
            }

            var heroes = HeroRepository.GetAll();
            if (heroes.Count == 0)
            {
                HeroRepository.Save(GenerateData.GetHeroes());
            }

            var answer = new
            {
                quests = quests.Count > 0 ? "Уже существует" : "Добавили",
                heroes = heroes.Count > 0 ? "Уже существует" : "Добавили",
                skills = skills.Count > 0 ? "Уже существует" : "Добавили",
            };

            return new JsonResult
            {
                Data = answer,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
