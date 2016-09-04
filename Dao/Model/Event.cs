using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class Event : BaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Desc { get; set; }

        //public List<Event> ParentEvents { get; set; }

        public List<Event> ChildrenEvents { get; set; }

        public Sex? RequrmentSex { get; set; } = null;
        public Race? RequrmentRace { get; set; } = null;
        public List<Skill> RequrmentSkill { get; set; }

        public Location RequrmentLocation { get; set; }
        //public Dictionary<CharacteristicType, long> RequrmentCharacteristics { get; set; }

        [Description("Add this value to total summ of quest effective")]
        public double ProgressChanging { get; set; } = 0;
        //public Dictionary<StatusType, long> CharacteristicsChanging { get; set; }

        public Quest Quest { get; set; }
    }
}