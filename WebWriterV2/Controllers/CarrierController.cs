using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Dao.IRepository;
using Dao.Model;

namespace WebWriterV2.Controllers
{
    public class CarrierController : Controller
    {
        //
        // GET: /Carrier/

        public IHeroRepository HeroRepository { get; set; }

        public CarrierController()
        {
            using (var scope = StaticContainer.Container.BeginLifetimeScope())
            {
                HeroRepository = scope.Resolve<IHeroRepository>();
            }
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Hero hero)
        {
            HeroRepository.SaveHero(hero);
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Profile(int heroId)
        {
            var model = HeroRepository.GetHero(heroId);

            if (model == null)
            {
                return RedirectToAction("Add");
            }

            return View(model);
        }

    }
}
