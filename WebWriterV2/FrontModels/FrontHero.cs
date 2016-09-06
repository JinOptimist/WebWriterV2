using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dao.Model;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class FrontHero
    {
        public FrontHero()
        {
        }

        public FrontHero(Hero hero)
        {
            Name = hero.Name;
            Background = hero.Background;
            Race = EnumHelper.GetFronEnum(typeof(Race), (long)hero.Race);
            Sex = EnumHelper.GetFronEnum(typeof(Sex), (long)hero.Sex);

            Characteristics = new List<FronEnumPlusValue>();
            foreach (var characteristic in hero.Characteristics)
            {
                var frontEnum = EnumHelper.GetFronEnum(typeof(CharacteristicType), (long)characteristic.CharacteristicType);
                Characteristics.Add(new FronEnumPlusValue(frontEnum, characteristic.Number));
            }

            Status = new List<FronEnumPlusValue>();
            //foreach (var state in hero.State)
            //{
            //    Status.Add(EnumHelper.GetFronEnum(typeof(StateType), (long)state.Type), state.Number);
            //}

            Skills = hero.Skills.Select(x => new FrontSkill(x)).ToList();
        }

        public string Name { get; set; }
        public string Background { get; set; }
        public FronEnum Race { get; set; }
        public FronEnum Sex { get; set; }

        public List<FronEnumPlusValue> Characteristics { get; set; }
        public List<FronEnumPlusValue> Status { get; set; }

        public List<FrontSkill> Skills { get; set; }
    }
}