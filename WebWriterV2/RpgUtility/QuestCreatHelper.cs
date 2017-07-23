using System.Collections.Generic;
using System.Linq;
using Dao.Model;

//using WebWriterV2.Models.rpg;

namespace WebWriterV2.RpgUtility
{
    public static class BookCreatHelper
    {
        public static void AddChildEvent(this Event currentEvent, Event addedEvent, string text = null)
        {
            ConnecteEvent(currentEvent, addedEvent, text);
        }

        public static void AddParentEvent(this Event currentEvent, Event addedEvent, string text = null)
        {
            ConnecteEvent(addedEvent, currentEvent, text);
        }

        public static void AddChildrenEvents(this Event currentEvent, params Event[] childrenEvents)
        {
            foreach(var childEvent in childrenEvents)
            {
                ConnecteEvent(currentEvent, childEvent);
            }
        }

        public static void AddParentsEvents(this Event currentEvent, string text = null, params Event[] parentsEvents)
        {
            foreach (var parentEvent in parentsEvents)
            {
                ConnecteEvent(parentEvent, currentEvent, text);
            }
        }

        private static void ConnecteEvent(Event parentEvent, Event childEvent, string text = null)
        {
            if (parentEvent.LinksFromThisEvent == null)
                parentEvent.LinksFromThisEvent = new List<EventLinkItem>();
            if (childEvent.LinksToThisEvent == null)
                childEvent.LinksToThisEvent = new List<EventLinkItem>();

            var eventLinkItem = new EventLinkItem
            {
                From = parentEvent,
                To = childEvent,
                Text = text ?? childEvent.Name
            };

            parentEvent.LinksFromThisEvent.Add(eventLinkItem);
            childEvent.LinksToThisEvent.Add(eventLinkItem);


        }
    }
}