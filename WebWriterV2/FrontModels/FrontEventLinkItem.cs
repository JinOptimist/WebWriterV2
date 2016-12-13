using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontEventLinkItem : BaseFront<EventLinkItem>
    {
        public FrontEventLinkItem()
        {
        }

        public FrontEventLinkItem(EventLinkItem eventLinkItemDb)
        {
            Id = eventLinkItemDb.Id;
            Text = eventLinkItemDb.Text;
            FromId = eventLinkItemDb.From.Id;
            ToId = eventLinkItemDb.To.Id;
        }

        public string Text { get; set; }
        public long FromId { get; set; }
        public long ToId { get; set; }

        public override EventLinkItem ToDbModel()
        {
            return new EventLinkItem
            {
                Id = Id,
                Text = Text,
                From = new Event {Id = FromId},
                To = new Event {Id = ToId},
            };
        }
    }
}