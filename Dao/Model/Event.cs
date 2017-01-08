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

        /// <summary>
        /// You must save this field separately. Use EventLinkItemRepository
        /// </summary>
        public virtual List<EventLinkItem> LinksFromThisEvent { get; set; }

        /// <summary>
        /// You must save this field separately. Use EventLinkItemRepository
        /// </summary>
        public virtual List<EventLinkItem> LinksToThisEvent { get; set; }

        /* Requirement */
        public virtual Sex? RequirementSex { get; set; } = null;
        public virtual Race? RequirementRace { get; set; } = null;
        public virtual List<Skill> RequirementSkill { get; set; }
        public virtual List<Characteristic> RequirementCharacteristics { get; set; }
        public virtual List<Thing> RequirementThings { get; set; }

        /* Changes */
        public virtual List<State> HeroStatesChanging { get; set; }
        public virtual List<Thing> ThingsChanges { get; set; }

        [Description("Add this value to total summ of quest effective")]
        public virtual double ProgressChanging { get; set; } = 0;
        //public Dictionary<StateType, long> CharacteristicsChanging { get; set; }


        //public virtual Location RequrmentLocation { get; set; }
        public virtual Quest Quest { get; set; }
        public virtual Quest ForRootQuest { get; set; }

        public void UpdateFrom(Event model)
        {
            if (Id != model.Id)
                throw new Exception($"You try update Event with Id: {model.Id} from Event with id {Id}");
            Name = model.Name;
            Desc = model.Desc;
            RequirementRace = model.RequirementRace;
            RequirementSex = model.RequirementSex;
            ProgressChanging = model.ProgressChanging;
            Quest = model.Quest;
            ForRootQuest = model.ForRootQuest;
        }
    }

    public enum RequirementType
    {
        More = 1,
        MoreOrEquals = 2,
        Less = 3,
        LessOrEquals = 4,
        Exist = 5,
        NotExist = 6,
        Equals = 7
    }
}