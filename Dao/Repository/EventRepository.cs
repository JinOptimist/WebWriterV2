using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        private readonly EventLinkItemRepository _eventLinkItemRepository;

        public const string RemoveExceptionMessage = "If you want remove event wich has children use method RemoveWholeBranch or RemoveEventAndChildren";
        public EventRepository(WriterContext db) : base(db)
        {
            _eventLinkItemRepository = new EventLinkItemRepository(db);
        }

        public override Event Save(Event model)
        {
            var newChildren = model.ChildrenEvents.ToList();

            // if we try update detached model
            if (Db.Entry(model).State == EntityState.Detached && model.Id > 0)
            {
                var modelFromDb = Entity.Find(model.Id);
                modelFromDb.UpdateFrom(model);
                model = modelFromDb;
            }

            //TODO how it work?
            newChildren.ForEach(x => model.ChildrenEvents.Remove(x));

            foreach (var child in newChildren)
            {
                //var addedChild = Entity.Find(child.Id) ?? child.Event;
                var addedChild = _eventLinkItemRepository.Entity.Find(child.Id) ?? child;
                model.ChildrenEvents.Add(addedChild);
            }

            if (model.Quest.RootEvent == null)
            {
                model.Quest.RootEvent = model;
            }

            return base.Save(model);
        }

        public override void Remove(Event currentEvent)
        {
            if (currentEvent == null)
                return;

            if (HasChild(currentEvent.Id))
            {
                throw new Exception(RemoveExceptionMessage);
            }

            base.Remove(currentEvent);
        }

        public void RemoveEventAndChildren(long currentEventId)
        {
            RemoveEventAndChildren(Get(currentEventId));
        }

        public void RemoveEventAndChildren(Event currentEvent)
        {
            if (currentEvent == null)
                return;
            currentEvent.ChildrenEvents = currentEvent.ChildrenEvents ?? new List<EventLinkItem>();

            if (currentEvent.ChildrenEvents.Any())
            {
                var forDelete = currentEvent.ChildrenEvents.Select(x => x.To).ToList();
                forDelete.ForEach(RemoveEventAndChildren);
            }

            var isDetached = Db.Entry(currentEvent).State == EntityState.Detached;
            if (isDetached)
            {
                return;
            }

            foreach (var parentEvent in currentEvent.ParentEvents)
            {
                var linkItem = parentEvent.To.ChildrenEvents.FirstOrDefault(x => x.To.Id == parentEvent.To.Id);
                parentEvent.To.ChildrenEvents.Remove(linkItem);
            }
            //currentEvent.ParentEvents.ForEach(x => x.Event.ChildrenEvents.Remove(currentEvent));
            base.Remove(currentEvent);
        }

        /// <summary>
        /// Special realization for cascade deleting
        /// </summary>
        /// <param name="currentEvent"></param>
        public void RemoveWholeBranch(long currentEventId) {
            RemoveWholeBranch(Get(currentEventId));
        }

        /// <summary>
        /// Special realization for cascade deleting
        /// </summary>
        /// <param name="currentEvent"></param>
        public void RemoveWholeBranch(Event currentEvent)
        {
            if (currentEvent == null)
                return;
            currentEvent.ChildrenEvents = currentEvent.ChildrenEvents ?? new List<EventLinkItem>();

            if (currentEvent.ChildrenEvents.Any())
            {
                var forDelete = currentEvent.ChildrenEvents.Select(x => x.To).ToList();
                forDelete.ForEach(RemoveWholeBranch);
            }

            var isDetached = Db.Entry(currentEvent).State == EntityState.Detached;
            if (isDetached)
            {
                return;
            }

            if (currentEvent.ChildrenEvents.Any())
            {
                currentEvent.ChildrenEvents = null;
                Save(currentEvent);
            }

            var parentEvents = currentEvent.ParentEvents.ToList();
            foreach (var someEventLink in parentEvents)
            {
                var linkItem = someEventLink.To.ChildrenEvents.FirstOrDefault(x => x.To.Id == currentEvent.Id);
                someEventLink.To.ChildrenEvents.Remove(linkItem);
                Save(someEventLink.To);
            }

            base.Remove(currentEvent);
        }

        public List<Event> GetAllEventsByQuest(long questId)
        {
            return Entity.Where(x => x.Quest.Id == questId).ToList();
        }

        public List<Event> GetRootEvents(long questId)
        {
            return Entity.Where(x => x.Quest != null && x.Quest.Id == questId).ToList();
        }

        public bool HasChild(long eventId)
        {
            return Entity.Any(x => x.Id == eventId && x.ChildrenEvents.Any());
        }
    }
}