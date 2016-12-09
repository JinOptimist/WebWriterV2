using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class Event : BaseModel, IUpdatable<Event>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Desc { get; set; }

        public virtual List<Event> ParentEvents { get; set; }

        public virtual List<Event> ChildrenEvents { get; set; }

        public virtual Sex? RequrmentSex { get; set; } = null;
        public virtual Race? RequrmentRace { get; set; } = null;
        public virtual List<Skill> RequrmentSkill { get; set; }
        public virtual List<Characteristic> RequrmentCharacteristics { get; set; }
        public virtual List<State> HeroStatesChanging { get; set; }

        public virtual Location RequrmentLocation { get; set; }


        [Description("Add this value to total summ of quest effective")]
        public virtual double ProgressChanging { get; set; } = 0;
        //public Dictionary<StateType, long> CharacteristicsChanging { get; set; }

        public virtual Quest Quest { get; set; }
        public virtual Quest ForRootQuest { get; set; }

        public void UpdateFrom(Event model)
        {
            Name = model.Name;
            Desc = model.Desc;
            RequrmentSex = model.RequrmentSex;
            RequrmentRace = model.RequrmentRace;
            ProgressChanging = model.ProgressChanging;

            Quest = model.Quest;
            ForRootQuest = model.ForRootQuest;
        }
    }
}