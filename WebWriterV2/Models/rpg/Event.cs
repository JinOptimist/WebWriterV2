using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WebWriterV2.Models.rpg
{
    public class Event
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }

        public List<Event> ParentEvents { get; set; }

        public Sex? RequrmentSex { get; set; } = null;
        public Race? RequrmentRace { get; set; } = null;
        public List<Skill> RequrmentSkill { get; set; } = null;
        //not null
        public Location RequrmentLocation { get; set; }
        //public Dictionary<CharacteristicType, long> RequrmentCharacteristics { get; set; }

        public double ProgressChanging { get; set; } = 0;
        public Dictionary<StatusType, long> CharacteristicsChanging { get; set; }
    }
}