using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dao.Model
{
    public class ChapterLinkItem : BaseModel, IUpdatable<ChapterLinkItem>
    {
        [Required]
        public string Text { get; set; }

        /* Requirement */
        public virtual List<Thing> RequirementThings { get; set; }
        public virtual List<State> RequirementStates { get; set; }

        /* Changes */
        public virtual List<Thing> ThingsChanges { get; set; }
        public virtual List<State> HeroStatesChanging { get; set; }

        /* Link reference */
        public virtual Chapter From { get; set; }
        public virtual Chapter To { get; set; }

        public void UpdateFrom(ChapterLinkItem model)
        {
            if (Id != model.Id)
                throw new Exception($"You try update EventLinkItem with Id: {model.Id} from EventLinkItem with id {Id}");
            Text = model.Text;
            From = model.From;
            To = model.To;
        }
    }
}