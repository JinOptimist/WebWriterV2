using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Dao.Model;
//using WebWriterV2.Models.rpg;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.RpgUtility
{
    public static class GenerateData
    {
        public const string Hp = "Hp";
        public const string MaxHp = "MaxHp";
        public const string Mp = "Mp";
        public const string MaxMp = "MaxMp";
        public const string Dodge = "Dodge";
        public const string Gold = "Gold";

        public const string Strength = "Сила";
        public const string Agility = "Ловкость";
        public const string Charism = "Красота";

        public const string SchoolBaseSkillName = "Базовые умения";
        public const string SchoolColdSkillName = "Школа льда";
        public const string SchoolFireSkillName = "Школа пламени";
        public const string SchoolNiceSkillName = "Соблазнения";

        public static Guild GetGuild(List<Hero> heroes, List<SkillSchool> skillSchools)
        {
            var guild = new Guild
            {
                Name = "Пьяный Бобры",
                Desc = "Основанна стареющим, но некогда велики воином, трижды спасшим мир, два двыжды убившим саму смерть ну и так по мелочи подвигов",
                Heroes = heroes,
                Influence = 10,
                Gold = 954,
                TrainingRooms = GenerateTrainingRooms(skillSchools).ToList()//.Where(x => x.School.Name == "Школа льда")
            };

            var location = new Location
            {
                Coordinate = new Point(0, 0),
                Guild = guild,
                Name = "Разваливающаяся платина",
                Desc = "Старая развалюха в которую страшно зайти. Не удивительно что только самые отчаяные оборванцы осмеливаются искать тут дом",
                //HeroesInLocation = GetHeroes()
            };
            guild.Location = location;

            return guild;
        }

        public static List<Hero> GetHeroes(List<Skill> skills, List<CharacteristicType> characteristicTypes, List<StateType> stateTypes)
        {
            var baseSkills = skills.Where(x => x.School.Name == SchoolBaseSkillName).ToList();

            var freeman = new Hero
            {
                Name = "Freeman",
                Race = Race.Человек,
                Sex = Sex.Муж,
                Characteristics = GenerateStat(characteristicTypes, 1),
                Background = "Сын физика ядерщика",
                Skills = new List<Skill>()
            };
            freeman.Skills.AddRange(baseSkills);
            freeman.Skills.Add(skills[0]);
            freeman.Skills.Add(skills[1]);
            freeman.SetDefaultState(stateTypes);

            var shani = new Hero
            {
                Name = "Шани",
                Race = Race.Эльф,
                Sex = Sex.Жен,
                Characteristics = GenerateStat(characteristicTypes, 2),
                Background = "Дочь проститутки",
                Skills = new List<Skill>()
            };
            shani.Skills.AddRange(baseSkills);
            shani.Skills.Add(skills[3]);
            shani.Skills.Add(skills[4]);
            shani.SetDefaultState(stateTypes);

            var ogrimar = new Hero
            {
                Name = "Огримар",
                Race = Race.Орк,
                Sex = Sex.Скрывает,
                Characteristics = GenerateStat(characteristicTypes, 3),
                Background = "В свои 14 трижды убивал",
                Skills = new List<Skill>()
            };
            ogrimar.Skills.AddRange(baseSkills);
            ogrimar.SetDefaultState(stateTypes);

            var result = new List<Hero>
            {
                freeman,
                shani,
                ogrimar
            };

            return result;
        }

        public static List<Characteristic> GenerateStat(List<CharacteristicType> characteristicTypes, int seed = 0)
        {
            var rnd = new Random(DateTime.Now.Millisecond + seed);
            var result = characteristicTypes.Select(characteristicType =>
                new Characteristic { CharacteristicType = characteristicType, Number = rnd.Next(1, 10) })
                .ToList();
            return result;
        }

        public static Quest GetQuest()
        {
            var quest = new Quest
            {
                Name = "Убить крыс",
                Desc = "Владелец амбара разметил заказ на убийство крыс. Отлично задание для новичка",
                Effective = 0,
            };
            //GenerateEventsForQuest(quest);
            return quest;
        }

        public static List<EventLinkItem> CreateConnectionForEvents(List<Event> events, Event currentEvent)
        {
            if (events.Count != GenerateEventsForQuest(null).Count)
                return null;

            List<EventLinkItem> result = null;

            var lvl0Event0 = events[0];
            var lvl1Event1 = events[1];
            var lvl1Event2 = events[2];
            var lvl1Event3 = events[3];
            var lvl1Event4 = events[4];
            var lvl2Event1 = events[5];
            var lvl2Event2 = events[6];
            var lvl2Event3 = events[7];
            var lvl3Event0 = events[8];

            switch (currentEvent.Name)
            {
                case "Начало":
                {
                        result = lvl0Event0.AddChildrenEvents(lvl1Event1, lvl1Event2, lvl1Event3);
                        break;
                }
                case "Перевёл дыхание":
                    {
                        result = lvl1Event4.AddParentsEvents(lvl1Event1, lvl1Event2, lvl1Event3);
                        result.AddRange(lvl1Event4.AddChildrenEvents(lvl2Event1, lvl2Event2, lvl2Event3));
                        break;
                    }
                case "Конец":
                    {
                        result = lvl3Event0.AddParentsEvents(lvl2Event1, lvl2Event2, lvl2Event3);
                        break;
                    }
            }

            return result;
        }

        public static List<Event> GenerateEventsForQuest(Quest quest)
        {
            // Tips
            // Desc = "У заказчика всегда была репутацию падкого на женское внимание мужика",
            // Desc = "В общем орков не любят со времён третьей общей войны, но порой можно встретить общины с прямо противоположным мнением",

            var lvl0Event0 = new Event
            {
                Name = "Начало",
                //ParentEvents = null,
                ProgressChanging = 30,
                Desc =
                    "Герой по не опытности целый день плутал по городу в поисках торговца, который сделал заказ на убийство крыс",
            };
            var lvl1Event1 = new Event
            {
                Name = "Баба заигрывает",
                ProgressChanging = 50,
                RequrmentSex = Sex.Жен,
                Desc =
                    "Героиня легко и непренуждённо пообщалась с заказчиком, после чего смогла убедить его снизить требования к выполнению задания",
            };
            var lvl1Event2 = new Event
            {
                Name = "Мужика послали",
                ProgressChanging = -30,
                RequrmentSex = Sex.Муж,
                Desc =
                    "Герой не успел и представиться как заказчик с недовольством начал указывать на недопустимость подобного поведения. Пришлось узнавать детали о задание у простых служащих, на что ушло в два раза больше сил и времени"
            };
            var lvl1Event3 = new Event
            {
                Name = "Скрытен работает",
                ProgressChanging = 10,
                RequrmentSex = Sex.Скрывает,
                Desc =
                    "Заказчик долго рассматривал героя, то недовольно бурча под нос, но натягивая приветливую улыбку. Тем временем вся необходимая информация была полученна и герой отправился дальше"
            };

            var lvl1Event4 = new Event
            {
                Name = "Перевёл дыхание",
                ProgressChanging = 10,
                Desc = "Получив все необходимые детали, герой уже было решил отправиться на прямую к амбару в котором были замечены крысы, но вспомнил наставления учителя. \"Что мы говорим основному квесту? Не сегодня\" и решил ещё день побродить по окрестностям в поисках приключений для своей мягкой точки",
            };

            var lvl2Event1 = new Event
            {
                Name = "Эльфу сложней",
                ProgressChanging = -30,
                RequrmentRace = Race.Эльф,
                Desc =
                    "Герою постоянно строили козни местные жители. Пришлось потратить много времени и сил на убеждение в своей добропорядочности",
            };
            var lvl2Event2 = new Event
            {
                Name = "Орку легче",
                ProgressChanging = 50,
                RequrmentRace = Race.Орк,
                Desc =
                    "Все вокруг пытались помочь Герою. Ничего полезного местные рассказать не смогли, но вот снаряжения надавали отменного, что явно помогло"
            };
            var lvl2Event3 = new Event
            {
                Name = "Человеку пофиг",
                ProgressChanging = 0,
                Desc = "Пользы от бесцельно проведённого дня было мало, зато герой успокаивал себя мыслью, что точно не пропустил возможных приключений. Возможно в следующий раз ему повезёт больше"
            };

            var lvl3Event0 = new Event
            {
                Name = "Конец",
                ProgressChanging = 30,
                Desc = "Герой нашёл злосчастных крыс и безжалостно уничтожил всех кого смог догнать",
            };

            //lvl0Event0.AddChildrenEvents(lvl1Event1, lvl1Event2, lvl1Event3);
            //lvl1Event4.AddParentsEvents(lvl1Event1, lvl1Event2, lvl1Event3);
            //lvl1Event4.AddChildrenEvents(lvl2Event1, lvl2Event2, lvl2Event3);
            //lvl3Event0.AddParentsEvents(lvl2Event1, lvl2Event2, lvl2Event3);

            var list = new List<Event>();
            list.Add(lvl0Event0);

            list.Add(lvl1Event1);
            list.Add(lvl1Event2);
            list.Add(lvl1Event3);
            list.Add(lvl1Event4);

            list.Add(lvl2Event1);
            list.Add(lvl2Event2);
            list.Add(lvl2Event3);

            list.Add(lvl3Event0);

            list.ForEach(x => x.Quest = quest);

            //quest.RootEvent = lvl0Event0;

            return list;
        }

        public static List<Skill> GenerateSkills(List<SkillSchool> schools, List<StateType> stateTypes)
        {
            var baseSchool = schools.FirstOrDefault(x => x.Name == "Базовые умения");
            var coldSchool = schools.FirstOrDefault(x => x.Name == "Школа льда");
            var fireSchool = schools.FirstOrDefault(x => x.Name == "Школа пламени");
            var niceSchool = schools.FirstOrDefault(x => x.Name == "Соблазнения");

            var hp = stateTypes.First(x => x.Name == Hp);
            var mp = stateTypes.First(x => x.Name == Mp);

            var skills = new List<Skill>();

            /* ************ Fire ************ */
            skills.Add(new Skill
            {
                Name = "Fire ball",
                Desc = "Fire ball",
                School = fireSchool,
                SelfChanging = new List<State> { new State { StateType = mp, Number = -4 } },
                TargetChanging = new List<State> { new State { StateType = hp, Number = -6 } }
            });

            skills.Add(new Skill
            {
                Name = "Pyro blast",
                Desc = "Pyro blast",
                School = fireSchool,
                SelfChanging = new List<State> { new State { StateType = mp, Number = -10 } },
                TargetChanging = new List<State> { new State { StateType = hp, Number = -10 } },
            });

            /* ************ Cold ************ */
            skills.Add(new Skill
            {
                Name = "Ice spear",
                Desc = "Ice spear",
                School = coldSchool,
                SelfChanging = new List<State> { new State { StateType = mp, Number = -2 } },
                TargetChanging = new List<State> { new State { StateType = hp, Number = -3 } },
            });

            skills.Add(new Skill
            {
                Name = "Ice armor",
                Desc = "Ice armor",
                School = coldSchool,
                SelfChanging = new List<State>
                {
                    new State { StateType = mp, Number = -4 },
                    new State { StateType = hp, Number = 10 }
                }
            });

            /* ************ Seduction ************ */
            skills.Add(new Skill
            {
                Name = "69",
                Desc = "No question, please. It's working and that all what you need to know",
                School = niceSchool,
            });

            /* ************ Base ************ */
            skills.Add(new Skill
            {
                Name = "Удар рукой",
                Desc = "Что может быть проще?",
                School = baseSchool,
                TargetChanging = new List<State> { new State { StateType = hp, Number = -2 } },
            });

            skills.Add(new Skill
            {
                Name = "Блок щитом",
                Desc = "Кто хочет жить, использует щит",
                School = baseSchool,
                SelfChanging = new List<State> { new State { StateType = hp, Number = 2 } },
            });

            skills.Add(new Skill
            {
                Name = "Уворот",
                Desc = "Для тех кто хочет умереть красиво",
                School = baseSchool,
                SelfChanging = new List<State> { new State { StateType = hp, Number = 1 } },
            });

            return skills;
        }

        public static List<TrainingRoom> GenerateTrainingRooms(List<SkillSchool> schools)
        {
            var baseSchool = schools.FirstOrDefault(x => x.Name == "Базовые умения");
            var coldSchool = schools.FirstOrDefault(x => x.Name == "Школа льда");
            var fireSchool = schools.FirstOrDefault(x => x.Name == "Школа пламени");
            var niceSchool = schools.FirstOrDefault(x => x.Name == "Соблазнения");

            var rooms = new List<TrainingRoom>();

            rooms.Add(new TrainingRoom
            {
                Name = "Жаровня",
                School = fireSchool,
                Price = 300
            });

            rooms.Add(new TrainingRoom
            {
                Name = "Холодильник",
                School = coldSchool,
                Price = 120
            });

            rooms.Add(new TrainingRoom
            {
                Name = "Бар",
                School = niceSchool,
                Price = 500
            });

            rooms.Add(new TrainingRoom
            {
                Name = "Комната посвящения",
                School = baseSchool,
                Price = 10
            });

            return rooms;
        }

        public static List<StateType> GenerateStateTypes()
        {
            var stateTypes = new List<StateType>();

            stateTypes.Add(new StateType
            {
                Name = MaxHp,
                Desc = "Сколько куриц не ешь, больше жизней в тебя просто не вместится",
            });

            stateTypes.Add(new StateType
            {
                Name = MaxMp,
                Desc = "Сколько зелей не пей, больше маны в тебя просто не вместится",
            });

            stateTypes.Add(new StateType
            {
                Name = Hp,
                Desc = "Опустить до нуля и ты труп",
            });

            stateTypes.Add(new StateType
            {
                Name = Mp,
                Desc = "Нет маны, нет заклинаний",
            });

            stateTypes.Add(new StateType
            {
                Name = Gold,
                Desc = "Что может быть лучше кошелька с золотыми? Мешок с золотыми!",
            });

            stateTypes.Add(new StateType
            {
                Name = Dodge,
                Desc = "Борис хрен попадёшь, на всегда останеться недостижимым идеалом",
            });

            return stateTypes;
        }

        public static List<CharacteristicType> GenerateCharacteristicType(List<StateType> stateTypes)
        {
            var maxHp = stateTypes.First(x => x.Name == MaxHp);
            var hp = stateTypes.First(x => x.Name == Hp);
            var dodge = stateTypes.First(x => x.Name == Dodge);
            var gold = stateTypes.First(x => x.Name == Gold);
            var characteristicType = new List<CharacteristicType>();

            characteristicType.Add(new CharacteristicType
            {
                Name = Strength,
                Desc = "Чем сильней, тем тупей, шутят над орками. А те почему-то смеются и бьют себя в голову",
                EffectState = new List<State>
                {
                    new State { StateType = maxHp, Number = 5 },
                    new State { StateType = hp, Number = 5 }
                }
            });

            characteristicType.Add(new CharacteristicType
            {
                Name = Agility,
                Desc = "Удержать яйцо на иголке? Легко!",
                EffectState = new List<State> { new State { StateType = dodge, Number = 5 } }
            });

            characteristicType.Add(new CharacteristicType
            {
                Name = Charism,
                Desc = "Это не честно кричала некрасивая девочка, глядя как выходит за муж очередная подруга",
                EffectState = new List<State> { new State { StateType = gold, Number = 10 } }
            });

            return characteristicType;
        }

        public static List<SkillSchool> GenerateSchools()
        {
            var schools = new List<SkillSchool>();

            schools.Add(new SkillSchool
            {
                Name = SchoolBaseSkillName,
                Desc = "Доступны всем по умолчанию. Ну разве что кроме совсем 'одарённых'"
            });

            schools.Add(new SkillSchool
            {
                Name = SchoolColdSkillName,
                Desc = "Охладись и начинай"
            });

            schools.Add(new SkillSchool
            {
                Name = SchoolFireSkillName,
                Desc = "Для тех кто любит зажигать"
            });

            schools.Add(new SkillSchool
            {
                Name = SchoolNiceSkillName,
                Desc = "Всё то что поможет вам убедить людей, без грубой силой"
            });

            return schools;
        }

        public static Hero GetDefaultHero(List<StateType> stateTypes, List<CharacteristicType> characteristicTypes, List<Skill> skills)
        {
            var hero = new Hero();

            hero.State = stateTypes.Select(x => new State { Number = 1, StateType = x }).ToList();
            hero.Characteristics = characteristicTypes.Select(x => new Characteristic { Number = 1, CharacteristicType = x }).ToList();
            hero.Skills = skills;

            return hero;
        }
    }
}
