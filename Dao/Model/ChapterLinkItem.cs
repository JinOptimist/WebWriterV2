using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dal.Model
{
    public class ChapterLinkItem : BaseModel, IUpdatable<ChapterLinkItem>
    {
        public string Text { get; set; }

        public virtual List<StateRequirement> StateRequirement { get; set; }
        public virtual List<StateChange> StateChanging { get; set; }

        /* Link reference */
        public virtual Chapter From { get; set; }
        public virtual Chapter To { get; set; }

        /// <summary>
        /// We want use to calculated statistic
        /// </summary>
        public virtual List<TravelStep> TravelSteps { get; set; }

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