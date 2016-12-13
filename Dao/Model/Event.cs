using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dao.Model
{
    public class Event : BaseModel, IUpdatable<Event>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Desc { get; set; }

        public virtual List<EventLinkItem> ParentEvents { get; set; }
        public virtual List<EventLinkItem> ChildrenEvents { get; set; }

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
            if (Id != model.Id)
                throw new Exception($"You try update Event with Id: {model.Id} from Event with id {Id}");
            Name = model.Name;
            Desc = model.Desc;
            RequrmentRace = model.RequrmentRace;
            RequrmentSex = model.RequrmentSex;
            ProgressChanging = model.ProgressChanging;
            Quest = model.Quest;
            ForRootQuest = model.ForRootQuest;
        }
    }
}