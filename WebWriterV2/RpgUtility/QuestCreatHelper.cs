using System.Collections.Generic;
using Dao.Model;

//using WebWriterV2.Models.rpg;

namespace WebWriterV2.RpgUtility
{
    public static class QuestCreatHelper
    {
        public static Event AddEvent(this Event parentEvent, Event addedEvent)
        {
            parentEvent.ChildrenEvents.Add(addedEvent);
            addedEvent.ParentEvents.Add(parentEvent);
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
            if (parentEvent.ChildrenEvents == null)
                parentEvent.ChildrenEvents = new List<Event>();
            if (childEvent.ParentEvents == null)
                childEvent.ParentEvents = new List<Event>();

            parentEvent.ChildrenEvents.Add(childEvent);
            childEvent.ParentEvents.Add(parentEvent);
        }
    }
}