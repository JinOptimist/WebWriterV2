using System.Collections.Generic;
using Dao.Model;

//using WebWriterV2.Models.rpg;

namespace WebWriterV2.RpgUtility
{
    public static class QuestCreatHelper
    {
        public static Event AddEvent(this Event parentEvent, Event addedEvent)
        {
            parentEvent.EventLinkItems.Add(new EventLinkItem
            {
                From = parentEvent,
                To = addedEvent,
                Text = addedEvent.Name
            });
            //addedEvent.ParentEvents.Add(parentEvent);
            return parentEvent;
        }

        public static void AddChildrenEvents(this Event currentEvent, params Event[] childrenEvents)
        {
            foreach (var childEvent in childrenEvents)
            {
                ConnecteEvent(currentEvent, childEvent);
            }
        }

        public static void AddParentsEvents(this Event currentEvent, params Event[] parentsEvents)
        {
            foreach (var parentEvent in parentsEvents)
            {
                ConnecteEvent(parentEvent, currentEvent);
            }
        }

        private static void ConnecteEvent(Event parentEvent, Event childEvent)
        {
            if (parentEvent.EventLinkItems == null)
                parentEvent.EventLinkItems = new List<EventLinkItem>();
            //if (childEvent.ParentEvents == null)
            //    childEvent.ParentEvents = new List<EventLinkItem>();

            parentEvent.EventLinkItems.Add(new EventLinkItem
            {
                From = parentEvent,
                To = childEvent,
                Text = childEvent.Name
            });

            //childEvent.ParentEvents.Add(new EventLinkItem
            //{
            //    From = parentEvent,
            //    To = childEvent,
            //    Text = childEvent.Name
            //});
        }
    }
}