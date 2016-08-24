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
                    Stats = GenerateStat(1),
                    Background = "Сын физика ядерщика"
                },
                new Hero
                {
                    Name = "Шани",
                    Race = Race.Elf,
                    Sex = Sex.Female,
                    Stats = GenerateStat(2),
                    Background = "Дочь проститутки"
                },
                new Hero
                {
                    Name = "Огримар",
                    Race = Race.Orc,
                    Sex = Sex.Unknown,
                    Stats = GenerateStat(3),
                    Background = "В свои 14 трижды убивал"
                }
            };
            return result;
        }

        private List<Stat> GenerateStat(int seed = 0)
        {
            var result = new List<Stat>();
            var rnd = new Random(DateTime.Now.Millisecond + seed);
            result.Add(new Stat {Name = StatList.Strength, Value = rnd.Next(1, 10) });
            result.Add(new Stat { Name = StatList.Agility, Value = rnd.Next(1, 10) });
            result.Add(new Stat { Name = StatList.Charism, Value = rnd.Next(1, 10) });
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
                    Wiles = GenerateWilesForQuest()
                }
            };
        }

        private List<Wile> GenerateWilesForQuest()
        {
            var result = new List<Wile>
            {
                new Wile
                {
                    Desc = "У заказчика всегда была репутацию падкого на женское внимание мужика",
                    Events = new List<Event>
                    {
                        new Event
                        {
                            ProgressChanging = 50,
                            RequrmentSex = Sex.Female,
                            Desc = "Героиня легко и непренуждённо пообщалась с заказчиком, после чего смогла убедить его снизить требования к выполнению задания",
                        },
                        new Event
                        {
                            ProgressChanging = -30,
                            RequrmentSex = Sex.Male,
                            Desc = "Герой не успел и представиться как заказчик с недовольством начал указывать на недопустимость подобного поведения"
                        }
                    }
                },
                new Wile
                {
                    Desc = "В общем орков не любят со времён третьей общей войны, но порой можно встретить общины с прямо противоположным мнением",
                    Events = new List<Event>
                    {
                        new Event
                        {
                            ProgressChanging = -30,
                            RequrmentRace = Race.Elf,
                            Desc = "Герою постоянно строили козни местные жители. Пришлось потратить много времени на поиски локации",

                        },
                        new Event
                        {
                            ProgressChanging = 50,
                            RequrmentRace = Race.Orc,
                            Desc = "Все вокруг пытались помочь Герою. Ничего полезного местные рассказать не смогли, но вот снаряжения надавали отменного, что явно помогло"
                        }
                    }
                }
            };

            return result;
        }
    }
}
