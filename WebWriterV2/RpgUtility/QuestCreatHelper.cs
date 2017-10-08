using System.Collections.Generic;
using System.Linq;
using Dao.Model;

//using WebWriterV2.Models.rpg;

namespace WebWriterV2.RpgUtility
{
    public static class BookCreatHelper
    {
        public static void AddChildEvent(this Chapter currentEvent, Chapter addedEvent, string text = null)
        {
            ConnecteEvent(currentEvent, addedEvent, text);
        }

        public static void AddParentEvent(this Chapter currentEvent, Chapter addedEvent, string text = null)
        {
            ConnecteEvent(addedEvent, currentEvent, text);
        }

        public static void AddChildrenEvents(this Chapter currentEvent, params Chapter[] childrenEvents)
        {
            foreach(var childEvent in childrenEvents)
            {
                ConnecteEvent(currentEvent, childEvent);
            }
        }

        public static void AddParentsEvents(this Chapter currentEvent, string text = null, params Chapter[] parentsEvents)
        {
            foreach (var parentEvent in parentsEvents)
            {
                ConnecteEvent(parentEvent, currentEvent, text);
            }
        }

        private static void ConnecteEvent(Chapter parentEvent, Chapter childEvent, string text = null)
        {
            if (parentEvent.LinksFromThisChapter == null)
                parentEvent.LinksFromThisChapter = new List<EventLinkItem>();
            if (childEvent.LinksToThisChapter == null)
                childEvent.LinksToThisChapter = new List<EventLinkItem>();

            var eventLinkItem = new EventLinkItem
            {
                From = parentEvent,
                To = childEvent,
                Text = text ?? childEvent.Name
            };

            parentEvent.LinksFromThisChapter.Add(eventLinkItem);
            childEvent.LinksToThisChapter.Add(eventLinkItem);


        }
    }
}