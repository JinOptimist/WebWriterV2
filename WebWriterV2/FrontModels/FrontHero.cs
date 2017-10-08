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
            Name = hero.Name;
            Background = hero.Background;

            State = hero.State.Select(x => new FrontState(x)).ToList();
            Inventory = hero.Inventory?.Select(x => new FrontThing(x)).ToList();
            CurrentEvent = hero.CurrentChapter == null ? null : new FrontEvent(hero.CurrentChapter);
        }

        public string Name { get; set; }
        public string Background { get; set; }

        public List<FrontState> State { get; set; }
        public List<FrontThing> Inventory { get; set; }

        public FrontEvent CurrentEvent { get; set; }

        public override Hero ToDbModel()
        {
            var hero = new Hero
            {
                Id = Id,
                Name = Name,
                Background = Background,
                State = State.Select(x => x.ToDbModel()).ToList(),
                Inventory = Inventory?.Select(x => x.ToDbModel()).ToList(),
                CurrentChapter = CurrentEvent?.ToDbModel()
            };

            hero.Inventory?.ForEach(x => x.Hero = hero);

            return hero;
        }
    }
}