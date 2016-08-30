using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebWriterV2.Models.rpg;
using WebWriterV2.Utility;

namespace WebWriterV2.Controllers
{
    public class RpgController : Controller
    {
        //
        // GET: /Rpg/

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
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(listNameValue),
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
                Data = SerializeHelper.Serialize(GetHeroesTemp()),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetRandomQuest()
        {
            return new JsonResult
            {
                Data = SerializeHelper.Serialize(GetQuestsTemp().First()),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetGuildInfo()
        {
            return new JsonResult
            {
                Data = SerializeHelper.Serialize(GetGuildTemp()),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #region Временно. Вместо база дынных.

        private Guild GetGuildTemp()
        {
            var guild = new Guild
            {
                Name = "Пьяный Бобры",
                Desc = "Основанна стареющим, но некогда велики воином, трижды спасшим мир, два двыжды убившим саму смерть ну и так по мелочи подвигов",
                Heroes = GetHeroesTemp(),
                Influence = 10,
                Gold = 954
            };

            var location = new Location
            {
                Coordinate = new Point(0, 0),
                Guild = guild,
                Name = "Разваливающаяся платина",
                Desc = "Старая развалюха в которую страшно зайти. Не удивительно что только самые отчаяные оборванцы осмеливаются искать тут дом",
                HeroesInLocation = GetHeroesTemp()
            };
            //guild.Location = location;

            return guild;
        }

        private List<Hero> GetHeroesTemp()
        {
            var result = new List<Hero>
            {
                new Hero
                {
                    Name = "Freeman",
                    Race = Race.Человек,
                    Sex = Sex.Муж,
                    Characteristics = GenerateStat(1),
                    Background = "Сын физика ядерщика"
                },
                new Hero
                {
                    Name = "Шани",
                    Race = Race.Эльф,
                    Sex = Sex.Жен,
                    Characteristics = GenerateStat(2),
                    Background = "Дочь проститутки"
                },
                new Hero
                {
                    Name = "Огримар",
                    Race = Race.Орк,
                    Sex = Sex.Скрывает,
                    Characteristics = GenerateStat(3),
                    Background = "В свои 14 трижды убивал"
                }
            };
            return result;
        }

        private Dictionary<CharacteristicType, long> GenerateStat(int seed = 0)
        {
            var result = new Dictionary<CharacteristicType, long>();
            var rnd = new Random(DateTime.Now.Millisecond + seed);
            result.Add(CharacteristicType.Strength, rnd.Next(1, 10));
            result.Add(CharacteristicType.Agility, rnd.Next(1, 10));
            result.Add(CharacteristicType.Charism, rnd.Next(1, 10));
            return result;
        }

        private List<Quest> GetQuestsTemp()
        {
            return new List<Quest>
            {
                new Quest
                {
                    Name = "Убить крыс",
                    Desc = "Владелец амбара разметил заказ на убийство крыс. Отлично задание для новичка",
                    QuestEvents = GenerateEventsForQuest()
                }
            };
        }

        private List<Event> GenerateEventsForQuest()
        {
            // Tips
            // Desc = "У заказчика всегда была репутацию падкого на женское внимание мужика",
            // Desc = "В общем орков не любят со времён третьей общей войны, но порой можно встретить общины с прямо противоположным мнением",
            var lvl0Event0 = new Event
            {
                Name = "Начало",
                ParentEvents = null,
                ProgressChanging = 30,
                Desc =
                    "Герой по не опытности целый день плутал по городу в поисках торговца, который сделал заказ на убийство крыс",
            };
            var lvl1Event1 = new Event
            {
                Name = "Баба заигрывает",
                ParentEvents = new List<Event>() { lvl0Event0 },
                ProgressChanging = 50,
                RequrmentSex = Sex.Жен,
                Desc =
                    "Героиня легко и непренуждённо пообщалась с заказчиком, после чего смогла убедить его снизить требования к выполнению задания",
            };
            var lvl1Event2 = new Event
            {
                Name = "Мужика послали",
                ParentEvents = new List<Event>() { lvl0Event0 },
                ProgressChanging = -30,
                RequrmentSex = Sex.Муж,
                Desc =
                    "Герой не успел и представиться как заказчик с недовольством начал указывать на недопустимость подобного поведения. Пришлось узнавать детали о задание у простых служащих, на что ушло в два раза больше сил и времени"
            };
            var lvl1Event3 = new Event
            {
                Name = "Скрытен работает",
                ParentEvents = new List<Event>() { lvl0Event0 },
                ProgressChanging = 10,
                RequrmentSex = Sex.Скрывает,
                Desc =
                    "Заказчик долго рассматривал героя, то недовольно бурча под нос, но натягивая приветливую улыбку. Тем временем вся необходимая информация была полученна и герой отправился дальше"
            };

            var lvl1Event4 = new Event
            {
                Name = "Перевёл дыхание",
                ParentEvents = new List<Event> {lvl1Event1, lvl1Event2, lvl1Event3},
                ProgressChanging = 10,
                Desc = "Получив все необходимые детали, герой уже было решил отправиться на прямую к амбару в котором были замечены крысы, но вспомнил наставления учителя. \"Что мы говорим основному квесту? Не сегодня\" и решил ещё день побродить по окрестностям в поисках приключений для своей мягкой точки",
            };

            var lvl2Event1 = new Event
            {
                Name = "Эльфу сложней",
                ParentEvents = new List<Event>() { lvl1Event4 },
                ProgressChanging = -30,
                RequrmentRace = Race.Эльф,
                Desc =
                    "Герою постоянно строили козни местные жители. Пришлось потратить много времени и сил на убеждение в своей добропорядочности",
            };
            var lvl2Event2 = new Event
            {
                Name = "Орку легче",
                ParentEvents = new List<Event>() { lvl1Event4 },
                ProgressChanging = 50,
                RequrmentRace = Race.Орк,
                Desc =
                    "Все вокруг пытались помочь Герою. Ничего полезного местные рассказать не смогли, но вот снаряжения надавали отменного, что явно помогло"
            };
            var lvl2Event3 = new Event
            {
                Name = "Человеку пофиг",
                ParentEvents = new List<Event>() { lvl1Event4 },
                ProgressChanging = 0,
                Desc = "Пользы от бесцельно проведённого дня было мало, зато герой успокаивал себя мыслью, что точно не пропустил возможных приключений. Возможно в следующий раз ему повезёт больше"
            };

            var lvl3Event0 = new Event
            {
                Name = "Конец",
                ParentEvents = new List<Event> { lvl2Event1, lvl2Event2, lvl2Event3 },
                ProgressChanging = 30,
                Desc = "Герой нашёл злосчастных крыс и безжалостно уничтожил всех кого смог догнать",
            };

            var result = new List<Event>
            {
                lvl0Event0,

                lvl1Event1,
                lvl1Event2,
                lvl1Event3,
                lvl1Event4,

                lvl2Event1,
                lvl2Event2,
                lvl2Event3,

                lvl3Event0
            };

            var id = 1;
            foreach (var myEvent in result)
            {
                myEvent.Id = id++;
            }

            return result;
        }

        #endregion
    }
}
