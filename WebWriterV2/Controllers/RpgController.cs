using System;
using System.Collections.Generic;
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
using Dao.Repository;
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
        public IGuildRepository GuildRepository { get; set; }

        private readonly WriterContext _context;

        public RpgController()
        {
            _context = new WriterContext();

            EventRepository = new EventRepository(_context);
            QuestRepository = new QuestRepository(_context);
            HeroRepository = new HeroRepository(_context);
            SkillRepository = new SkillRepository(_context);
            GuildRepository = new GuildRepository(_context);

            //using (var scope = StaticContainer.Container.BeginLifetimeScope())
            //{
            //    EventRepository = scope.Resolve<IEventRepository>();
            //    QuestRepository = scope.Resolve<IQuestRepository>();
            //    HeroRepository = scope.Resolve<IHeroRepository>();
            //    SkillRepository = scope.Resolve<ISkillRepository>();
            //}
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

        /* ************** Guild ************** */
        public JsonResult GetGuildInfo()
        {
            var guildFromDb = GuildRepository.GetAll().First();
            var schools = guildFromDb.TrainingRooms.Select(x => x.School).ToList();
            var heroes = HeroRepository.GetList(guildFromDb.Heroes.Select(x => x.Id));
            guildFromDb.Heroes = heroes;
            var skillsBySchool = SkillRepository.GetBySchools(schools);
            var frontGuild = new FrontGuild(guildFromDb, skillsBySchool);
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(frontGuild),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult QuestCompleted(int guildId, int gold)
        {
            var answer = "+";
            try
            {
                var guild = GuildRepository.Get(guildId);
                guild.Gold += gold;
                GuildRepository.Save(guild);
            }
            catch (Exception e)
            {
                answer = e.Message;
            }

            return new JsonResult
            {
                Data = answer,
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

        public JsonResult AddSkillToHero(int heroId, int skillId)
        {
            var answer = "+";
            try
            {
                var hero = HeroRepository.Get(heroId);
                var skill = SkillRepository.Get(skillId);
                hero.Skills.Add(skill);
                HeroRepository.Save(hero);
            }
            catch (Exception e)
            {
                answer = e.Message;
            }

            return new JsonResult
            {
                Data = answer,
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
            if (!quests.Any())
            {
                var quest = GenerateData.GetQuest();
                QuestRepository.Save(quest);
            }

            var skills = SkillRepository.GetAll();
            if (!skills.Any())
            {
                var generateSkills = GenerateData.GenerateSkills();
                SkillRepository.Save(generateSkills);
            }

            var heroes = HeroRepository.GetAll();
            if (!heroes.Any())
            {
                heroes = GenerateData.GetHeroes();
                foreach (var hero in heroes)
                {
                    var skillsFromDb = hero.Skills.Select(skill => SkillRepository.GetByName(skill.Name)).ToList();
                    hero.Skills = skillsFromDb;
                }

                HeroRepository.Save(heroes);
            }

            var guilds = GuildRepository.GetAll();
            if (!guilds.Any())
            {
                var guild = GenerateData.GetGuild();
                guild.Heroes = heroes;
                GuildRepository.Save(guild);
            }

            var answer = new
            {
                quests = quests.Any() ? "Уже существует" : "Добавили",
                heroes = heroes.Any() ? "Уже существует" : "Добавили",
                skills = skills.Any() ? "Уже существует" : "Добавили",
                guilds = guilds.Any() ? "Уже существует" : "Добавили",
            };

            return new JsonResult
            {
                Data = answer,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult ReInit()
        {
            var guilds = GuildRepository.GetAll();
            GuildRepository.Remove(guilds);

            var events = EventRepository.GetRootEvents();
            EventRepository.Remove(events);

            var quests = QuestRepository.GetAll();
            QuestRepository.Remove(quests);

            var heroes = HeroRepository.GetAll();
            HeroRepository.Remove(heroes);

            return Init();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}
