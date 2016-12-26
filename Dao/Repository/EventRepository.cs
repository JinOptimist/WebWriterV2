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
        private readonly Lazy<EventLinkItemRepository> _eventLinkItemRepository;

        public const string RemoveExceptionMessage = "If you want remove event wich has children use method RemoveWholeBranch or RemoveEventAndChildren";
        public EventRepository(WriterContext db) : base(db)
        {
            _eventLinkItemRepository = new Lazy<EventLinkItemRepository>(() => new EventLinkItemRepository(db));
        }

        public override Event Save(Event model)
        {
            // if we try update detached model
            var entry = Db.Entry(model);
            if (entry.State == EntityState.Detached && model.Id > 0)
            {
                var modelFromDb = Entity.Find(model.Id);
                modelFromDb.UpdateFrom(model);
                model = modelFromDb;
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

            if (currentEvent.LinksFromThisEvent?.Any() ?? false)
            {
                _eventLinkItemRepository.Value.Remove(currentEvent.LinksFromThisEvent);
            }
            if (currentEvent.LinksToThisEvent?.Any() ?? false)
            {
                _eventLinkItemRepository.Value.Remove(currentEvent.LinksToThisEvent);
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
            
            if (currentEvent.LinksFromThisEvent?.Any() ?? false)
            {
                var forDelete = currentEvent.LinksFromThisEvent.Select(x => x.From).ToList();
                _eventLinkItemRepository.Value.Remove(currentEvent.LinksFromThisEvent);
                forDelete.ForEach(RemoveEventAndChildren);
            }

            if (currentEvent.LinksToThisEvent.Any())
            {
                _eventLinkItemRepository.Value.Remove(currentEvent.LinksToThisEvent);
            }

            var isDetached = Db.Entry(currentEvent).State == EntityState.Detached;
            if (isDetached)
            {
                return;
            }

            //foreach (var parentEvent in currentEvent.EventLinkItems)
            //{
            //    var linkItem = parentEvent.To.ChildrenEvents.FirstOrDefault(x => x.To.Id == parentEvent.To.Id);
            //    parentEvent.To.ChildrenEvents.Remove(linkItem);
            //}
            //currentEvent.ParentEvents.ForEach(x => x.Event.EventLinkItems.Remove(currentEvent));
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
            currentEvent.LinksFromThisEvent = currentEvent.LinksFromThisEvent ?? new List<EventLinkItem>();

            if (currentEvent.LinksFromThisEvent.Any())
            {
                var forDelete = currentEvent.LinksFromThisEvent.Select(x => x.From).ToList();
                forDelete.ForEach(RemoveWholeBranch);
            }

            var isDetached = Db.Entry(currentEvent).State == EntityState.Detached;
            if (isDetached)
            {
                return;
            }

            if (currentEvent.LinksFromThisEvent.Any())
            {
                currentEvent.LinksFromThisEvent = null;
                Save(currentEvent);
            }

            //var parentEvents = currentEvent.ParentEvents.ToList();
            //foreach (var someEventLink in parentEvents)
            //{
            //    var linkItem = someEventLink.To.ChildrenEvents.FirstOrDefault(x => x.To.Id == currentEvent.Id);
            //    someEventLink.To.ChildrenEvents.Remove(linkItem);
            //    Save(someEventLink.To);
            //}

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

        public List<Event> GetEndingEvents(long questId)
        {
            return Entity.Where(x => x.Quest != null && x.Quest.Id == questId
                                     && x.LinksFromThisEvent.Count == 0).ToList();
        }

        public bool HasChild(long eventId)
        {
            return Entity.Any(x => x.Id == eventId && x.LinksFromThisEvent.Any());
        }

        public List<Event> GetNotAvailableEvents(long questId)
        {
            return Entity.Where(x => x.Quest != null && x.Quest.Id == questId
                                     && x.LinksToThisEvent.Count == 0 && x.ForRootQuest == null).ToList();
        }
    }
}