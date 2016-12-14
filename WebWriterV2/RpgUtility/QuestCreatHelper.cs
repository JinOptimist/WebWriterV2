using System.Collections.Generic;
using System.Linq;
using Dao.Model;

//using WebWriterV2.Models.rpg;

namespace WebWriterV2.RpgUtility
{
    public static class QuestCreatHelper
    {
        public static Event AddEvent(this Event parentEvent, Event addedEvent)
        {
            parentEvent.LinksFromThisEvent.Add(new EventLinkItem
            {
                From = parentEvent,
                To = addedEvent,
                Text = addedEvent.Name
            });
            //addedEvent.ParentEvents.Add(parentEvent);
            return parentEvent;
        }

        public static List<EventLinkItem> AddChildrenEvents(this Event currentEvent, params Event[] childrenEvents)
        {
            return childrenEvents.Select(childEvent => ConnecteEvent(currentEvent, childEvent)).ToList();
        }

        public static List<EventLinkItem> AddParentsEvents(this Event currentEvent, params Event[] parentsEvents)
        {
            return parentsEvents.Select(parentEvent => ConnecteEvent(parentEvent, currentEvent)).ToList();
        }

        private static EventLinkItem ConnecteEvent(Event parentEvent, Event childEvent)
        {
            //if (parentEvent.LinksFromThisEvent == null)
            //    parentEvent.LinksFromThisEvent = new List<EventLinkItem>();
            //if (childEvent.LinksToThisEvent == null)
            //    childEvent.LinksToThisEvent = new List<EventLinkItem>();

            //parentEvent.LinksFromThisEvent.Add(a);


            var eventLinkItem = new EventLinkItem
            {
                From = parentEvent,
                To = childEvent,
                Text = childEvent.Name
            };
            return eventLinkItem;

            //childEvent.LinksToThisEvent.Add(new EventLinkItem
            //{
            //    From = parentEvent,
            //    To = childEvent,
            //    Text = childEvent.Name
            //});

            //childEvent.ParentEvents.Add(new EventLinkItem
            //{
            //    From = parentEvent,
            //    To = childEvent,
            //    Text = childEvent.Name
            //});
        }
    }
}