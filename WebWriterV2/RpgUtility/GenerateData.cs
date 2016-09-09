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
        public static Guild GetGuild()
        {
            var guild = new Guild
            {
                Name = "Пьяный Бобры",
                Desc = "Основанна стареющим, но некогда велики воином, трижды спасшим мир, два двыжды убившим саму смерть ну и так по мелочи подвигов",
                Heroes = GetHeroes(),
                Influence = 10,
                Gold = 954,
                TrainingRooms = GenerateTrainingRooms().Where(x => x.School == SkillSchool.Cold).ToList()
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

        public static List<Hero> GetHeroes()
        {
            //skill count 8
            var skills = GenerateSkills();
            var freeman = new Hero
            {
                Name = "Freeman",
                Race = Race.Человек,
                Sex = Sex.Муж,
                Characteristics = GenerateStat(1),
                Background = "Сын физика ядерщика",
                Skills = new List<Skill>()
            };
            freeman.Skills.AddRange(skills.Where(x => x.School == SkillSchool.Base));
            freeman.Skills.Add(GenerateSkills()[0]);
            freeman.Skills.Add(GenerateSkills()[1]);
            freeman.SetDefaultState();

            var shani = new Hero
            {
                Name = "Шани",
                Race = Race.Эльф,
                Sex = Sex.Жен,
                Characteristics = GenerateStat(2),
                Background = "Дочь проститутки",
                Skills = new List<Skill>()
            };
            shani.Skills.AddRange(skills.Where(x => x.School == SkillSchool.Base));
            shani.Skills.Add(GenerateSkills()[3]);
            shani.Skills.Add(GenerateSkills()[4]);
            shani.SetDefaultState();

            var ogrimar = new Hero
            {
                Name = "Огримар",
                Race = Race.Орк,
                Sex = Sex.Скрывает,
                Characteristics = GenerateStat(3),
                Background = "В свои 14 трижды убивал",
                Skills = new List<Skill>()
            };
            ogrimar.Skills.AddRange(skills.Where(x => x.School == SkillSchool.Base));
            ogrimar.SetDefaultState();

            var result = new List<Hero>
            {
                freeman,
                shani,
                ogrimar
            };

            return result;
        }

        public static List<Characteristic> GenerateStat(int seed = 0)
        {
            var result = new List<Characteristic>();
            var rnd = new Random(DateTime.Now.Millisecond + seed);
            result.Add(new Characteristic { CharacteristicType = CharacteristicType.Strength, Number = rnd.Next(1, 10) });
            result.Add(new Characteristic { CharacteristicType = CharacteristicType.Agility, Number = rnd.Next(1, 10) });
            result.Add(new Characteristic { CharacteristicType = CharacteristicType.Charism, Number = rnd.Next(1, 10) });
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

            GenerateEventsForQuest(quest);

            return quest;
        }

        public static void GenerateEventsForQuest(Quest quest)
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

            lvl0Event0.AddChildrenEvents(lvl1Event1, lvl1Event2, lvl1Event3);
            lvl1Event4.AddParentsEvents(lvl1Event1, lvl1Event2, lvl1Event3);
            lvl1Event4.AddChildrenEvents(lvl2Event1, lvl2Event2, lvl2Event3);
            lvl3Event0.AddParentsEvents(lvl2Event1, lvl2Event2, lvl2Event3);
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
            quest.AllEvents = list;
            quest.RootEvent = lvl0Event0;
        }

        public static List<Skill> GenerateSkills()
        {
            var skills = new List<Skill>();

            /* ************ Fire ************ */
            skills.Add(new Skill
            {
                Name = "Fire ball",
                Desc = "Fire ball",
                School = SkillSchool.Fire,
                SelfChanging = new List<State> { new State { StateType = StateType.CurrentMp, Number = -4 } },
                TargetChanging = new List<State> { new State { StateType = StateType.CurrentHp, Number = -6 } }
            });

            skills.Add(new Skill
            {
                Name = "Pyro blast",
                Desc = "Pyro blast",
                School = SkillSchool.Fire,
                SelfChanging = new List<State> { new State { StateType = StateType.CurrentMp, Number = -10 } },
                TargetChanging = new List<State> { new State { StateType = StateType.CurrentHp, Number = -10 } },
            });

            /* ************ Cold ************ */
            skills.Add(new Skill
            {
                Name = "Ice spear",
                Desc = "Ice spear",
                School = SkillSchool.Cold,
                SelfChanging = new List<State> { new State { StateType = StateType.CurrentMp, Number = -2 } },
                TargetChanging = new List<State> { new State { StateType = StateType.CurrentHp, Number = -3 } },
            });

            skills.Add(new Skill
            {
                Name = "Ice armor",
                Desc = "Ice armor",
                School = SkillSchool.Cold,
                SelfChanging = new List<State> { new State { StateType = StateType.CurrentMp, Number = -4 } },
                TargetChanging = new List<State> { new State { StateType = StateType.CurrentHp, Number = 10 } },
            });

            /* ************ Seduction ************ */
            skills.Add(new Skill
            {
                Name = "69",
                Desc = "No question, please. It's working and that all what you need to know",
                School = SkillSchool.Seduction,
            });

            /* ************ Base ************ */
            skills.Add(new Skill
            {
                Name = "Удар рукой",
                Desc = "Что может быть проще?",
                School = SkillSchool.Base,
                TargetChanging = new List<State> { new State { StateType = StateType.CurrentHp, Number = -2 } },
            });

            skills.Add(new Skill
            {
                Name = "Блок щитом",
                Desc = "Кто хочет жить, использует щит",
                School = SkillSchool.Base,
                TargetChanging = new List<State> { new State { StateType = StateType.CurrentHp, Number = -2 } },
            });

            skills.Add(new Skill
            {
                Name = "Уворот",
                Desc = "Для тех кто хочет умереть красиво",
                School = SkillSchool.Base,
                TargetChanging = new List<State> { new State { StateType = StateType.CurrentHp, Number = -2 } },
            });

            return skills;
        }

        public static List<TrainingRoom> GenerateTrainingRooms()
        {
            var rooms = new List<TrainingRoom>();

            rooms.Add(new TrainingRoom
            {
                Name = "Жаровня",
                School = SkillSchool.Fire,
                Price = 300
            });

            rooms.Add(new TrainingRoom
            {
                Name = "Холодильник",
                School = SkillSchool.Cold,
                Price = 120
            });

            rooms.Add(new TrainingRoom
            {
                Name = "Бар",
                School = SkillSchool.Seduction,
                Price = 500
            });

            rooms.Add(new TrainingRoom
            {
                Name = "Комната посвящения",
                School = SkillSchool.Base,
                Price = 10
            });

            return rooms;
        }
    }
}
