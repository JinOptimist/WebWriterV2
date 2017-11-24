using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dao.Model;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class FrontHero : BaseFront<Hero>
    {
        public FrontHero()
        {
        }

        public FrontHero(Hero hero)
        {
            Id = hero.Id;
            State = hero.State.Select(x => new FrontState(x)).ToList();
            CurrentEvent = hero.CurrentChapter == null ? null : new FrontChapter(hero.CurrentChapter);
        }

        public string Name { get; set; }

        public List<FrontState> State { get; set; }

        public FrontChapter CurrentEvent { get; set; }

        public override Hero ToDbModel()
        {
            var hero = new Hero
            {
                Id = Id,
                State = State.Select(x => x.ToDbModel()).ToList(),
                CurrentChapter = CurrentEvent?.ToDbModel()
            };

            return hero;
        }
    }
}