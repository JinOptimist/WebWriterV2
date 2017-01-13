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
        private int _priceOfRestore = 5;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly WriterContext _context = new WriterContext();

        public IEventRepository EventRepository { get; }
        public IEventLinkItemRepository EventLinkItemRepository { get; }
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
        public IThingSampleRepository ThingSampleRepository { get; set; }
        public IThingRepository ThingRepository { get; set; }

        public RpgController()
        {
            EventRepository = new EventRepository(_context);
            EventLinkItemRepository = new EventLinkItemRepository(_context);
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
            ThingSampleRepository = new ThingSampleRepository(_context);
            ThingRepository = new ThingRepository(_context);

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

        /* ************** Utils for enum ************** */
        public JsonResult GetListRace()
        {
            var listRace = Enum.GetValues(typeof(Race)).Cast<Race>();
            var listNameValue = listRace.Select(race => new FrontEnum(race));

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(listNameValue),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetListSex()
        {
            var listSex = Enum.GetValues(typeof(Sex)).Cast<Sex>();
            var listNameValue = listSex.Select(sex => new FrontEnum(sex));

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(listNameValue),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetListRequirementType()
        {
            var listRequirementType = Enum.GetValues(typeof(RequirementType)).Cast<RequirementType>();
            var listNameValue = listRequirementType.Select(requirementType => new FrontEnum(requirementType));

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(listNameValue),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /* ************** Guild ************** */
        public JsonResult GetGuild(long guildId)
        {
            var guild = GuildRepository.Get(guildId)
                // TODO Debug only!
                ?? GuildRepository.GetAll().First();

            var frontGuild = new FrontGuild(guild);
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
                var guild = GuildRepository.Get(guildId)
                    // TODO Debug only!
                    ?? GuildRepository.GetAll().First();
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
        public JsonResult GetHero(long heroId)
        {
            var hero = HeroRepository.Get(heroId);
            if (hero == null)
            {
                var stateTypes = StateTypeRepository.GetAll();
                hero = GenerateData.GetDefaultHero(stateTypes, null, null);
            }

            var a = hero.Inventory?.FirstOrDefault();

            var frontHero = new FrontHero(hero);
            return new JsonResult
            {
                Data = SerializeHelper.Serialize(frontHero),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

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
            var hero = HeroRepository.Get(id);
            var things = hero.Inventory.ToList();
            ThingRepository.Remove(things);

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
            var hero = HeroRepository.Get(heroId);
            var skill = SkillRepository.Get(skillId);
            var guild = GuildRepository.Get(hero.Guild.Id);
            hero.Skills.Add(skill);
            HeroRepository.Save(hero);

            guild.Gold -= skill.Price;
            GuildRepository.Save(guild);

            return new JsonResult
            {
                Data = SerializeHelper.Serialize(new FrontHero(hero)),
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

        public JsonResult RestoreHero(long heroId)
        {
            var hero = HeroRepository.Get(heroId);

            var hp = hero.State.First(x => x.StateType.Name == GenerateData.Hp);
            var maxHp = hero.State.First(x => x.StateType.Name == GenerateData.MaxHp);
            var mp = hero.State.First(x => x.StateType.Name == GenerateData.Mp);
            var maxMp = hero.State.First(x => x.StateType.Name == GenerateData.MaxMp);
            hp.Number = maxHp.Number;
            mp.Number = maxMp.Number;

            HeroRepository.Save(hero);

            var guild = DebugGetGuild();

            guild.Gold -= _priceOfRestore;
            GuildRepository.Save(guild);

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

        /* ************** Thing ************** */
        public JsonResult GetThingSamples()
        {
            var thingSamples = ThingSampleRepository.GetAll();
            var thingSamplesFront = thingSamples.Select(x => new FrontThingSample(x)).ToList();

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(thingSamplesFront),
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

        public JsonResult ChangeState(long stateId, long delta)
        {
            var state = StateRepository.Get(stateId);
            state.Number += delta;
            var fake = state.StateType;
            StateRepository.Save(state);

            var frontHero = new FrontState(state);
            return new JsonResult
            {
                Data = SerializeHelper.Serialize(frontHero),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /* ************** TraningRoom ************** */
        public JsonResult GetTraningRoom(long traningRoomId)
        {
            var room = TrainingRoomRepository.Get(traningRoomId);

            var frontRoom = new FrontTrainingRoom(room);

            return new JsonResult
            {
                Data = SerializeHelper.Serialize(frontRoom),
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
            var quest = QuestRepository.Get(id);
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
            var frontQuest = SerializeHelper.Deserialize<FrontQuest>(jsonQuest);
            var quest = frontQuest.ToDbModel();
            QuestRepository.Save(quest);
            frontQuest = new FrontQuest(quest);

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(frontQuest),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult ImportQuest(string jsonQuest)
        {
            var frontQuest = SerializeHelper.Deserialize<FrontQuest>(jsonQuest);
            var quest = frontQuest.ToDbModel();

            var questName = QuestRepository.GetByName(quest.Name);
            if (questName == null)
            {
                quest.Id = 0;
                quest.Executor = null;
                var characteristics = new List<Characteristic>();
                var things = new List<Thing>();
                var skills = new List<Skill>();
                var states = new List<State>();
                var linkItems = new List<EventLinkItem>();

                foreach (var @event in quest.AllEvents)
                {
                    if (quest.RootEvent.Id == @event.Id)
                    {
                        @event.ForRootQuest = quest;
                    }

                    var eventLinkItems = @event.LinksFromThisEvent;
                    eventLinkItems.AddRange(@event.LinksToThisEvent);
                    foreach (var eventLinkItem in eventLinkItems)
                    {
                        eventLinkItem.Id = 0;
                        eventLinkItem.To = quest.AllEvents.First(x => x.Id == eventLinkItem.To.Id);
                        eventLinkItem.From = quest.AllEvents.First(x => x.Id == eventLinkItem.From.Id);
                    }

                    linkItems.AddRange(eventLinkItems);
                    @event.Quest = quest;
                }

                foreach (var @event in quest.AllEvents)
                {
                    @event.Id = 0;
                    characteristics.AddRange(@event.RequirementCharacteristics ?? new List<Characteristic>());
                    things.AddRange(@event.RequirementThings ?? new List<Thing>());
                    things.AddRange(@event.ThingsChanges ?? new List<Thing>());
                    skills.AddRange(@event.RequirementSkill ?? new List<Skill>());
                    states.AddRange(@event.HeroStatesChanging ?? new List<State>());
                }

                /* Process Things connections */
                characteristics.AddRange(things.SelectMany(x => x.ThingSample.PassiveCharacteristics ?? new List<Characteristic>()));
                characteristics.AddRange(things.SelectMany(x => x.ThingSample.UsingEffectCharacteristics ?? new List<Characteristic>()));
                states.AddRange(things.SelectMany(x=>x.ThingSample.PassiveStates ?? new List<State>()));
                states.AddRange(things.SelectMany(x => x.ThingSample.UsingEffectState ?? new List<State>()));

                /* Process Skills connections */
                states.AddRange(skills.SelectMany(x => x.SelfChanging ?? new List<State>()));
                states.AddRange(skills.SelectMany(x => x.TargetChanging ?? new List<State>()));

                /* Process Characteristics connections */
                states.AddRange(characteristics.SelectMany(x => x.CharacteristicType.EffectState ?? new List<State>()));

                foreach (var thing in things)
                {
                    thing.Id = 0;
                    thing.Hero = null;
                    thing.ThingSample.Id = 0;
                }

                foreach (var characteristic in characteristics)
                {
                    characteristic.Id = 0;
                    characteristic.CharacteristicType.Id = 0;
                }

                var nbsp = (char)160;// code of nbsp
                var sp = (char)32;// code of simple space

                foreach (var skill in skills)
                {
                    skill.Id = 0;
                    skill.School.Id = 0;

                    var clearName = skill.Name.Replace(nbsp, sp);
                    skill.Name = clearName;
                }

                foreach (var state in states)
                {
                    state.Id = 0;
                    state.StateType.Id = 0;
                }


                states.ForEach(StateRepository.CheckAndSave);
                things.ForEach(ThingRepository.CheckAndSave);
                characteristics.ForEach(CharacteristicRepository.CheckAndSave);
                var newSkill = skills.Select(SkillRepository.CheckAndSave).ToList();
                foreach (var @event in quest.AllEvents)
                {
                    for (var i = 0; i < @event.RequirementSkill.Count; i++)
                    {
                        var requrmentSkillName = @event.RequirementSkill[i].Name;
                        @event.RequirementSkill[i] = newSkill.First(x => x.Name == requrmentSkillName);
                    }
                }

                foreach (var @event in quest.AllEvents)
                {
                    @event.LinksFromThisEvent = new List<EventLinkItem>();
                }

                QuestRepository.Save(quest);

                EventLinkItemRepository.Save(linkItems);
                EventLinkItemRepository.RemoveDuplicates();
            }

            return new JsonResult
            {
                Data = true,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult ChangeRootEvent(long questId, long eventId)
        {
            var quest = QuestRepository.Get(questId);
            var @event = EventRepository.Get(eventId);
            quest.RootEvent = @event;
            QuestRepository.Save(quest);

            var frontEvent = new FrontEvent(@event);

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(frontEvent),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        /* ************** Event ************** */
        public JsonResult GetEndingEvents(long questId)
        {
            var events = EventRepository.GetEndingEvents(questId);
            var frontEvents = events.Select(x => new FrontEvent(x)).ToList();
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(frontEvents),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetNotAvailableEvents(long questId)
        {
            var events = EventRepository.GetNotAvailableEvents(questId);
            var frontEvents = events.Select(x => new FrontEvent(x)).ToList();
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(frontEvents),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetEvents(long questId)
        {
            var events = EventRepository.GetAllEventsByQuest(questId);
            var frontEvents = events.Select(x => new FrontEvent(x)).ToList();
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(frontEvents),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetEvent(long id)
        {
            var eventFromDb = EventRepository.Get(id);
            var frontEvent = new FrontEvent(eventFromDb);
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(frontEvent),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetEventForTravel(long eventId, long heroId)
        {
            var eventDb = EventRepository.Get(eventId);
            var hero = HeroRepository.Get(heroId);
            eventDb.LinksFromThisEvent.FilterLink(hero);
            var frontEvent = new FrontEvent(eventDb);
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(frontEvent),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetEventForTravelWithHero(long eventId, string heroJson)
        {
            var eventDb = EventRepository.Get(eventId);
            var frontHero = JsonConvert.DeserializeObject<FrontHero>(heroJson);
            var hero = frontHero.ToDbModel();

            eventDb.LinksFromThisEvent.FilterLink(hero);
            eventDb.EventChangesApply(hero);

            var frontEvent = new FrontEvent(eventDb);
            frontHero = new FrontHero(hero);
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(new {frontEvent, frontHero}),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult EventChangesApplyToHero(long eventId, long heroId)
        {
            var eventDb = EventRepository.Get(eventId);
            var hero = HeroRepository.Get(heroId);

            eventDb.EventChangesApply(hero);

            HeroRepository.Save(hero);
            var frontHero = new FrontHero(hero);
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(frontHero),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SaveEvent(string jsonEvent, long questId)
        {
            var frontEvent = SerializeHelper.Deserialize<FrontEvent>(jsonEvent);
            var eventModel = frontEvent.ToDbModel();
            if (eventModel.Id == 0)
            {
                eventModel.Quest = QuestRepository.Get(questId);
            }

            var eventFromDb = EventRepository.Save(eventModel);
            var frontEvents = new FrontEvent(eventFromDb);
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(frontEvents),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SaveEventLink(string jsonEventLink)
        {
            var frontEvent = SerializeHelper.Deserialize<FrontEventLinkItem>(jsonEventLink);
            var eventLinkItem = frontEvent.ToDbModel();

            var linkItemFromDb = EventLinkItemRepository.Save(eventLinkItem);
            var frontEvents = new FrontEventLinkItem(linkItemFromDb);
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(frontEvents),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveEventLink(long eventLinkId)
        {
            EventLinkItemRepository.Remove(eventLinkId);
            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(true),
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

        public JsonResult AddSkillToEvent(long eventId, long skillId)
        {
            var eventFromDb = EventRepository.Get(eventId);
            var skill = SkillRepository.Get(skillId);

            eventFromDb.RequirementSkill.Add(skill);
            EventRepository.Save(eventFromDb);

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(true),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveSkillToEvent(long eventId, long skillId)
        {
            var eventFromDb = EventRepository.Get(eventId);
            var skill = SkillRepository.Get(skillId);

            eventFromDb.RequirementSkill.Remove(skill);
            EventRepository.Save(eventFromDb);

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(true),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddCharacteristicToEvent(long eventId, long characteristicTypeId,
            int characteristicValue, int requirementType)
        {
            var eventFromDb = EventRepository.Get(eventId);
            var characteristicType = CharacteristicTypeRepository.Get(characteristicTypeId);

            var characteristic =
                eventFromDb.RequirementCharacteristics.FirstOrDefault(
                    x => x.CharacteristicType.Id == characteristicType.Id)
                ?? new Characteristic
                {
                    CharacteristicType = characteristicType
                };

            characteristic.Number = characteristicValue;
            characteristic.RequirementType = (RequirementType)requirementType;

            // if new
            if (characteristic.Id < 1)
                eventFromDb.RequirementCharacteristics.Add(characteristic);
            CharacteristicRepository.Save(characteristic);
            EventRepository.Save(eventFromDb);

            var frontCharacteristic = new FrontCharacteristic(characteristic);

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(frontCharacteristic),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveCharacteristicFromEvent(long characteristicId)
        {
            CharacteristicRepository.Remove(characteristicId);

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(true),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddStateToEvent(long eventId, long stateTypeId, int stateValue)
        {
            var eventFromDb = EventRepository.Get(eventId);
            var stateType = StateTypeRepository.Get(stateTypeId);

            var state =
                eventFromDb.HeroStatesChanging.FirstOrDefault(
                    x => x.StateType.Id == stateType.Id)
                ?? new State
                {
                    StateType = stateType
                };

            state.Number = stateValue;

            // if new
            if (state.Id < 1)
                eventFromDb.HeroStatesChanging.Add(state);
            StateRepository.Save(state);
            EventRepository.Save(eventFromDb);

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(state),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveStateFromEvent(long stateId)
        {
            StateRepository.Remove(stateId);

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(true),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddRequirementThingToEvent(long eventId, long thingSampleId, int count)
        {
            var eventFromDb = EventRepository.Get(eventId);
            var thingSample = ThingSampleRepository.Get(thingSampleId);
            var thing = new Thing
            {
                Count = count,
                Hero = null,
                ThingSample = thingSample,
                ItemInUse = false
            };

            ThingRepository.Save(thing);
            eventFromDb.RequirementThings.Add(thing);
            EventRepository.Save(eventFromDb);

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(thing),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveRequirementThingFromEvent(long eventId, long thingId)
        {
            var eventFromDb = EventRepository.Get(eventId);
            var thing = ThingRepository.Get(thingId);
            eventFromDb.RequirementThings.Remove(thing);
            EventRepository.Save(eventFromDb);
            ThingRepository.Remove(thing);

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(true),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddThingChangesToEvent(long eventId, long thingSampleId, int count)
        {
            var eventFromDb = EventRepository.Get(eventId);
            var thingSample = ThingSampleRepository.Get(thingSampleId);
            var thing = new Thing
            {
                Count = count,
                Hero = null,
                ThingSample = thingSample,
                ItemInUse = false
            };

            ThingRepository.Save(thing);
            eventFromDb.ThingsChanges.Add(thing);
            EventRepository.Save(eventFromDb);

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(thing),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveThingChangesFromEvent(long eventId, long thingId)
        {
            var eventFromDb = EventRepository.Get(eventId);
            var thing = ThingRepository.Get(thingId);
            eventFromDb.ThingsChanges.Remove(thing);
            EventRepository.Save(eventFromDb);
            ThingRepository.Remove(thing);

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(true),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /* ************** Init Db ************** */
        public JsonResult Init()
        {
            /* Создаём StateType */
            var stateTypes = StateTypeRepository.GetAll();
            if (!stateTypes.Any())
            {
                stateTypes = GenerateData.GenerateStateTypes();
                StateTypeRepository.Save(stateTypes);
            }

            /* Создаём ThingSamples */
            var thingSamples = ThingSampleRepository.GetAll();
            if (!thingSamples.Any())
            {
                thingSamples = GenerateData.GenerateThingSample(stateTypes);
                ThingSampleRepository.Save(thingSamples);
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

            /* Создаём Квесты. Чистый без евентов */
            var quests = QuestRepository.GetAll();
            if (!quests.Any())
            {
                var quest1 = GenerateData.QuestRat();
                QuestRepository.Save(quest1);

                var quest2 = GenerateData.QuestTower(characteristicTypes, stateTypes, skills, thingSamples);
                QuestRepository.Save(quest2);
            }

            // Создаём Евенты с текстом но без связей
            //var events = EventRepository.GetAll();
            //if (!events.Any())
            //{
            //    events = GenerateData.GenerateEventsForQuest(quest);
            //    foreach (var eve in events)
            //    {
            //        EventRepository.Save(eve);
            //    }
            //}

            // Создаём связи между Евентами
            //var eventLinkItemsDb = EventLinkItemRepository.GetAll();
            //if (!eventLinkItemsDb.Any())
            //{
            //    foreach (var currentEvent in events)
            //    {
            //        var eventLinkItems = GenerateData.CreateConnectionForEvents(events, currentEvent);
            //        if (eventLinkItems != null)
            //            EventLinkItemRepository.Save(eventLinkItems);
            //    }
            //}

            /* Создаём Героев */
            var heroes = HeroRepository.GetAll();
            var heroExist = heroes.Any();
            if (!heroExist)
            {
                heroes = GenerateData.GetHeroes(skills, characteristicTypes, stateTypes, thingSamples);
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
                //eventLinkItemsDb = eventLinkItemsDb.Any() ? "Уже существует" : "Добавили",
                heroes = heroExist ? "Уже существует" : "Добавили",
                skills = skills.Any() ? "Уже существует" : "Добавили",
                guilds = guilds.Any() ? "Уже существует" : "Добавили",
                thingSamples = thingSamples.Any() ? "Уже существует" : "Добавили",
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

            var things = ThingRepository.GetAll();
            ThingRepository.Remove(things);

            var thingSamples = ThingSampleRepository.GetAll();
            ThingSampleRepository.Remove(thingSamples);

            var heroes = HeroRepository.GetAll();
            HeroRepository.Remove(heroes);

            var guilds = GuildRepository.GetAll();
            GuildRepository.Remove(guilds);

            var links = EventLinkItemRepository.GetAll();
            EventLinkItemRepository.Remove(links);

            var events = EventRepository.GetAll();
            EventRepository.Remove(events);

            var quests = QuestRepository.GetAll();
            QuestRepository.Remove(quests);

            var stateTypes = StateTypeRepository.GetAll();
            StateTypeRepository.Remove(stateTypes);

            return Init();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }

        private Guild DebugGetGuild()
        {
            var guildIdStr = Request.Cookies["guildId"]?.Value ?? "0";
            var guildId = 0;
            if (int.TryParse(guildIdStr, out guildId))
            {
                guildId = -1;
            }

            return guildId > 1
                ? GuildRepository.Get(guildId)
                : GuildRepository.GetAll().First();
        }
    }
}
