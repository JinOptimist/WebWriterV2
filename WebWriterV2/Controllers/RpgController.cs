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

        private readonly WriterContext _context = new WriterContext();

        public IEventRepository EventRepository { get; }
        public IQuestRepository QuestRepository { get; set; }
        public IHeroRepository HeroRepository { get; set; }
        public ISkillRepository SkillRepository { get; set; }
        public IGuildRepository GuildRepository { get; set; }
        public ISkillSchoolRepository SkillSchoolRepository { get; set; }
        public ITrainingRoomRepository TrainingRoomRepository { get; set; }
        public ICharacteristicRepository CharacteristicRepository { get; set; }
        public ICharacteristicTypeRepository CharacteristicTypeRepository { get; set; }
        public IStateRepository StateRepository { get; set; }
        public IStateTypeRepository StateTypeRepository { get; set; }

        public RpgController()
        {
            EventRepository = new EventRepository(_context);
            QuestRepository = new QuestRepository(_context);
            HeroRepository = new HeroRepository(_context);
            SkillRepository = new SkillRepository(_context);
            GuildRepository = new GuildRepository(_context);
            SkillSchoolRepository = new SkillSchoolRepository(_context);
            TrainingRoomRepository = new TrainingRoomRepository(_context);
            CharacteristicRepository = new CharacteristicRepository(_context);
            CharacteristicTypeRepository = new CharacteristicTypeRepository(_context);
            StateRepository = new StateRepository(_context);
            StateTypeRepository = new StateTypeRepository(_context);

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
            var listNameValue = listRace.Select(race => new FronEnum(race));

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(listNameValue),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetListSex()
        {
            var listSex = Enum.GetValues(typeof(Sex)).Cast<Sex>();
            var listNameValue = listSex.Select(sex => new FronEnum(sex));

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(listNameValue),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /* ************** Guild ************** */
        public JsonResult GetGuildInfo()
        {
            var guild = GuildRepository.GetAll().First();

            var schools = guild.TrainingRooms.Select(x => x.School).ToList();
            var skillsBySchool = SkillRepository.GetBySchools(schools);

            var frontGuild = new FrontGuild(guild, skillsBySchool);
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

        public JsonResult SaveHero(string jsonHero)
        {
            var frontHero = SerializeHelper.Deserialize<FrontHero>(jsonHero);
            var hero = frontHero.ToDbModel();

            var guild = GuildRepository.GetAll().First();
            hero.Guild = guild;

            HeroRepository.Save(hero);
            frontHero.Id = hero.Id;

            return new JsonResult
            {
                Data = SerializeHelper.Serialize(frontHero),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveHero(long id)
        {
            HeroRepository.Remove(id);
            return new JsonResult
            {
                Data = true,
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

        public JsonResult GetEnemy()
        {
            var heroes = HeroRepository.GetAll();
            var frontHero = new FrontHero(heroes.LastOrDefault());
            return new JsonResult
            {
                Data = SerializeHelper.Serialize(frontHero),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetDefaultHero()
        {
            var stateTypes = StateTypeRepository.GetAll();
            var characteristicTypes = CharacteristicTypeRepository.GetAll();

            var skillSchool = SkillSchoolRepository.GetByName(GenerateData.SchoolBaseSkillName);
            var skills = SkillRepository.GetBySchool(skillSchool);

            var hero = GenerateData.GetDefaultHero(stateTypes, characteristicTypes, skills);
            var frontHero = new FrontHero(hero);
            return new JsonResult
            {
                Data = SerializeHelper.Serialize(frontHero),
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
            var frontSkills = skillsFromDb.Select(x => new FrontSkill(x)).ToList();
            return new JsonResult
            {
                Data = SerializeHelper.Serialize(frontSkills),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetSkillsSchool()
        {
            var skillSchools = SkillSchoolRepository.GetAll();
            return new JsonResult
            {
                Data = SerializeHelper.Serialize(skillSchools),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveSkill(long skillId)
        {
            SkillRepository.Remove(skillId);
            return new JsonResult
            {
                Data = true,
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

        public JsonResult GetSkillEffect(long skillId)
        {
            var skillsFromDb = SkillRepository.Get(skillId);
            var frontSkill = new FrontSkill(skillsFromDb);
            return new JsonResult
            {
                Data = SerializeHelper.Serialize(frontSkill),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SaveSkill(string jsonSkill)
        {
            var frontSkill = SerializeHelper.Deserialize<FrontSkill>(jsonSkill);
            var skill = frontSkill.ToDbModel();

            SkillRepository.Save(skill);

            return new JsonResult
            {
                Data = SerializeHelper.Serialize(new FrontSkill(skill)),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /* ************** State ************** */
        public JsonResult GetStateTypes()
        {
            var stateTypes = StateTypeRepository.GetAll();

            return new JsonResult
            {
                Data = SerializeHelper.Serialize(stateTypes),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /* ************** Characteristic ************** */
        public JsonResult GetCharacteristicTypes()
        {
            var characteristicTypes = CharacteristicTypeRepository.GetAll();

            var frontCharacteristicTypes = characteristicTypes.Select(x => new FrontCharacteristicType(x)).ToList();

            return new JsonResult
            {
                Data = SerializeHelper.Serialize(frontCharacteristicTypes),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SaveCharacteristicType(string jsonCharacteristicType)
        {
            var frontCharacteristicType = SerializeHelper.Deserialize<FrontCharacteristicType>(jsonCharacteristicType);
            var characteristicType = frontCharacteristicType.ToDbModel();

            CharacteristicTypeRepository.Save(characteristicType);

            return new JsonResult
            {
                Data = SerializeHelper.Serialize(new FrontCharacteristicType(characteristicType)),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveCharacteristicType(long characteristicTypeId)
        {
            CharacteristicTypeRepository.Remove(characteristicTypeId);
            return new JsonResult
            {
                Data = true,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /* ************** Quest ************** */
        public JsonResult GetQuest(long id)
        {
            var quest = QuestRepository.GetWithRootEvent(id);
            var frontQuest = new FrontQuest(quest);
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(frontQuest),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetQuests()
        {
            var quests = QuestRepository.GetAll();
            var frontQuests = quests.Select(x => new FrontQuest(x)).ToList();
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(frontQuests),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetOneQuest()
        {
            var quest = QuestRepository.GetAllWithRootEvent().FirstOrDefault();
            var frontQuest = new FrontQuest(quest);
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(frontQuest),
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
            var events = EventRepository.GetByQuest(questId);
            var frontEvents = events.Select(x => new FrontEvent(x)).ToList();
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(frontEvents),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetEventChildren(long id)
        {
            var eventFromDb = EventRepository.Get(id);
            var frontEvents = new FrontEvent(eventFromDb);
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(frontEvents),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SaveEvent(string jsonEvent, long questId)
        {
            var frontEvent = SerializeHelper.Deserialize<FrontEvent>(jsonEvent);
            Event eventModel = frontEvent.ToDbModel();
            var quest = QuestRepository.Get(questId);
            eventModel.Quest = quest;

            var eventFromDb = EventRepository.Save(eventModel);
            var frontEvents = new FrontEvent(eventFromDb);
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(frontEvents),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveEvent(long eventId)
        {
            EventRepository.Remove(eventId);
            return new JsonResult
            {
                Data = true,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /* ************** Init Db ************** */
        public JsonResult Init()
        {
            /* Создаём Квесты (Event внутри) */
            var quests = QuestRepository.GetAll();
            if (!quests.Any())
            {
                var quest = GenerateData.GetQuest();
                QuestRepository.Save(quest);
            }

            /* Создаём StateType */
            var stateTypes = StateTypeRepository.GetAll();
            if (!stateTypes.Any())
            {
                stateTypes = GenerateData.GenerateStateTypes();
                StateTypeRepository.Save(stateTypes);
            }

            /* Создаём CharacteristicType */
            var characteristicTypes = CharacteristicTypeRepository.GetAll();
            if (!characteristicTypes.Any())
            {
                characteristicTypes = GenerateData.GenerateCharacteristicType(stateTypes);
                CharacteristicTypeRepository.Save(characteristicTypes);
            }

            /* Создаём Школы умений */
            var skillSchools = SkillSchoolRepository.GetAll();
            var skillSchoolsExist = skillSchools.Any();
            if (!skillSchoolsExist)
            {
                skillSchools = GenerateData.GenerateSchools();
                SkillSchoolRepository.Save(skillSchools);
            }

            /* Создаём Умения */
            var skills = SkillRepository.GetAll();
            if (!skills.Any())
            {
                skills = GenerateData.GenerateSkills(skillSchools, stateTypes);
                SkillRepository.Save(skills);
            }

            /* Создаём Героев */
            var heroes = HeroRepository.GetAll();
            var heroExist = heroes.Any();
            if (!heroExist)
            {
                heroes = GenerateData.GetHeroes(skills, characteristicTypes, stateTypes);
                HeroRepository.Save(heroes);
            }

            /* Создаём Гильдию */
            var guilds = GuildRepository.GetAll();
            if (!guilds.Any())
            {
                var guild = GenerateData.GetGuild(heroes, skillSchools);
                GuildRepository.Save(guild);
            }

            var answer = new
            {
                skillSchools = skillSchoolsExist ? "Уже существует" : "Добавили",
                quests = quests.Any() ? "Уже существует" : "Добавили",
                heroes = heroExist ? "Уже существует" : "Добавили",
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
            var trainingRooms = TrainingRoomRepository.GetAll();
            TrainingRoomRepository.Remove(trainingRooms);

            var states = StateRepository.GetAll();
            StateRepository.Remove(states);

            var characteristic = CharacteristicRepository.GetAll();
            CharacteristicRepository.Remove(characteristic);

            var characteristicTypes = CharacteristicTypeRepository.GetAll();
            CharacteristicTypeRepository.Remove(characteristicTypes);

            var skills = SkillRepository.GetAll();
            SkillRepository.Remove(skills);

            var skillSchools = SkillSchoolRepository.GetAll();
            SkillSchoolRepository.Remove(skillSchools);

            var heroes = HeroRepository.GetAll();
            HeroRepository.Remove(heroes);

            var guilds = GuildRepository.GetAll();
            GuildRepository.Remove(guilds);

            var events = EventRepository.GetRootEvents();
            EventRepository.Remove(events);

            var quests = QuestRepository.GetAll();
            QuestRepository.Remove(quests);

            return Init();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}
