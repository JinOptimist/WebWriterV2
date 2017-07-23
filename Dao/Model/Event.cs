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
        public virtual List<Thing> RequirementThings { get; set; }
        public virtual List<State> RequirementStates{ get; set; }

        /* Changes */
        public virtual List<Thing> ThingsChanges { get; set; }
        public virtual List<State> HeroStatesChanging { get; set; }

        [Description("Add this value to total summ of book effective")]
        public virtual double ProgressChanging { get; set; } = 0;

        //public virtual Location RequrmentLocation { get; set; }
        public virtual Book Book { get; set; }
        public virtual Book ForRootBook { get; set; }

        public void UpdateFrom(Event model)
        {
            if (Id != model.Id)
                throw new Exception($"You try update Event with Id: {model.Id} from Event with id {Id}");
            Name = model.Name;
            Desc = model.Desc;
            ProgressChanging = model.ProgressChanging;
            Book = model.Book;
            ForRootBook = model.ForRootBook;
        }
    }
}