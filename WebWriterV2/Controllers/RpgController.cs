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

        public ActionResult Index()
        {
            return View();
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
                    Background = "Сын физика ядерщика"
                },
                new Hero
                {
                    Name = "Шани",
                    Race = Race.Elf,
                    Sex = Sex.Female,
                    Background = "Дочь проститутки"
                },
                new Hero
                {
                    Name = "Огримар",
                    Race = Race.Orc,
                    Sex = Sex.Unknown,
                    Background = "В свои 14 трижды убивал"
                }
            };
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
                    PartsOfQuest = GeneratePartsOfQuest()
                }
            };
        }

        private List<PartOfQuest> GeneratePartsOfQuest()
        {
            var result = new List<PartOfQuest>
            {
                new PartOfQuest
                {
                    Desc = "У заказчика всегда была репутацию падкого на женское внимание мужика",
                    Events = new List<Event>
                    {
                        new Event
                        {
                            ProgressPlus = 50,
                            RequrmentSex = Sex.Female,
                            Desc = "Героиня легко и непренуждённо пообщалась с заказчиком, после чего смогла убедить его снизить требования к выполнению задания",
                        },
                        new Event
                        {
                            ProgressPlus = -30,
                            RequrmentSex = Sex.Male,
                            Desc = "Герой не успел и представиться как заказчик с недовольством начал указывать на недопустимость подобного поведения"
                        }
                    }
                },
                new PartOfQuest
                {
                    Desc = "Слухи о предвзятости к эльфам вполне могут оказать не слухами. Орки враждуют с Эльфами уже столетия, а судя по жутким корявкам, писал его именно Орк",
                    Events = new List<Event>
                    {
                        new Event
                        {
                            ProgressPlus = -30,
                            RequrmentRace = Race.Elf,
                            Desc = "С героем отказались разговаривать. Пришлось узнавать детали задания у прислуги",

                        },
                        new Event
                        {
                            ProgressPlus = 50,
                            RequrmentRace = Race.Orc,
                            Desc = "Героя сразу признали как своего. Ничего полезного орки рассказать не смогли, но вот снаряжения надавали отменного, что явно поможет в выполнения квеста"
                        }
                    }
                }
            };

            return result;
        }
    }
}
