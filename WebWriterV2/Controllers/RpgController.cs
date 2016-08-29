using System;
using System.Collections.Generic;
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

        public ActionResult AddQuest()
        {
            return View();
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

        private List<Hero> GetHeroesTemp()
        {
            var result = new List<Hero>
            {
                new Hero
                {
                    Name = "Freeman",
                    Race = Race.Human,
                    Sex = Sex.Male,
                    Characteristics = GenerateStat(1),
                    Background = "Сын физика ядерщика"
                },
                new Hero
                {
                    Name = "Шани",
                    Race = Race.Elf,
                    Sex = Sex.Female,
                    Characteristics = GenerateStat(2),
                    Background = "Дочь проститутки"
                },
                new Hero
                {
                    Name = "Огримар",
                    Race = Race.Orc,
                    Sex = Sex.Unknown,
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
                RequrmentSex = Sex.Female,
                Desc =
                    "Героиня легко и непренуждённо пообщалась с заказчиком, после чего смогла убедить его снизить требования к выполнению задания",
            };
            var lvl1Event2 = new Event
            {
                Name = "На мужика накричали",
                ParentEvents = new List<Event>() { lvl0Event0 },
                ProgressChanging = -30,
                RequrmentSex = Sex.Male,
                Desc =
                    "Герой не успел и представиться как заказчик с недовольством начал указывать на недопустимость подобного поведения. Пришлось узнавать детали о задание у простых служащих, на что ушло в два раза больше сил и времени"
            };
            var lvl1Event3 = new Event
            {
                Name = "Перевёл дыхание",
                ParentEvents = new List<Event> {lvl1Event1, lvl1Event2},
                ProgressChanging = 10,
                Desc = "Получив все необходимые детали, герой уже было решил отправиться на прямую к амбару в котором были замечены крысы, но вспомнил наставления учителя. \"Что мы говорим основному квесту? Не сегодня\" и решил ещё день побродить по окрестностям в поисках приключений для своей мягкой точки",
            };

            var lvl2Event1 = new Event
            {
                Name = "Эльфу сложней",
                ParentEvents = new List<Event>() { lvl1Event3 },
                ProgressChanging = -30,
                RequrmentRace = Race.Elf,
                Desc =
                    "Герою постоянно строили козни местные жители. Пришлось потратить много времени и сил на убеждение в своей добропорядочности",
            };
            var lvl2Event2 = new Event
            {
                Name = "Орку легче",
                ParentEvents = new List<Event>() { lvl1Event3 },
                ProgressChanging = 50,
                RequrmentRace = Race.Orc,
                Desc =
                    "Все вокруг пытались помочь Герою. Ничего полезного местные рассказать не смогли, но вот снаряжения надавали отменного, что явно помогло"
            };
            var lvl2Event3 = new Event
            {
                Name = "Человеку пофиг",
                ParentEvents = new List<Event>() { lvl1Event3 },
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
    }
}
