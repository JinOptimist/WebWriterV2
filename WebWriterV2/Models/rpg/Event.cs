using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebWriterV2.Models.rpg
{
    public class Event : BaseModel
    {
        public string Name { get; set; }
        public string Desc { get; set; }

        public List<Event> ParentEvents { get; set; } = new List<Event>();

        public Sex? RequrmentSex { get; set; } = null;
        public Race? RequrmentRace { get; set; } = null;
        public List<Skill> RequrmentSkill { get; set; } = null;
        //not null attr
        public Location RequrmentLocation { get; set; }
        //public Dictionary<CharacteristicType, long> RequrmentCharacteristics { get; set; }

        public double ProgressChanging { get; set; } = 0;
    }
}