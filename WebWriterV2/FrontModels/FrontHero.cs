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
            Race = new FrontEnum(hero.Race);
            Sex = new FrontEnum(hero.Sex);

            Characteristics = hero.Characteristics.Select(x => new FrontCharacteristic(x)).ToList();
            State = hero.State.Select(x => new FrontState(x)).ToList();
            Skills = hero.Skills.Select(x => new FrontSkill(x)).ToList();
            Inventory = hero.Inventory?.Select(x => new FrontThing(x)).ToList();
        }

        public string Name { get; set; }
        public string Background { get; set; }
        public FrontEnum Race { get; set; }
        public FrontEnum Sex { get; set; }

        public List<FrontCharacteristic> Characteristics { get; set; }
        public List<FrontState> State { get; set; }
        public List<FrontSkill> Skills { get; set; }
        public List<FrontThing> Inventory { get; set; }

        public override Hero ToDbModel()
        {
            var hero = new Hero
            {
                Name = Name,
                Background = Background,
                Sex = (Sex)Sex.Value,
                Race = (Race)Race.Value,
                State = State.Select(x => x.ToDbModel()).ToList(),
                Characteristics = Characteristics.Select(x => x.ToDbModel()).ToList(),
                Skills = Skills.Select(x => x.ToDbModel()).ToList(),
                Inventory = Inventory?.Select(x => x.ToDbModel()).ToList()
            };

            hero.Inventory?.ForEach(x => x.Hero = hero);

            return hero;
        }
    }
}