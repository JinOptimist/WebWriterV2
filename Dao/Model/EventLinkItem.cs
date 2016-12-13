using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dao.Model
{
    public class EventLinkItem : BaseModel, IUpdatable<EventLinkItem>
    {
        [Required]
        public string Text { get; set; }

        public virtual Event From { get; set; }

        public virtual Event To { get; set; }

        public void UpdateFrom(EventLinkItem model)
        {
            if (Id != model.Id)
                throw new Exception($"You try update EventLinkItem with Id: {model.Id} from EventLinkItem with id {Id}");
            Text = model.Text;
            From = model.From;
            To = model.To;
        }
    }
}