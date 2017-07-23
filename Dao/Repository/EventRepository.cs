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
        private readonly ThingRepository _thingRepository;
        private readonly StateRepository _stateRepository;
        private readonly HeroRepository _heroRepository;

        public const string RemoveExceptionMessage = "If you want remove event wich has children use method RemoveWholeBranch or RemoveEventAndChildren";
        public EventRepository(WriterContext db) : base(db)
        {
            _eventLinkItemRepository = new Lazy<EventLinkItemRepository>(() => new EventLinkItemRepository(db));
            _thingRepository = new ThingRepository(db);
            _stateRepository = new StateRepository(db);
            _heroRepository = new HeroRepository(db);
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

            if (model.Book.RootEvent == null)
            {
                model.Book.RootEvent = model;
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
            if (currentEvent.ThingsChanges?.Any() ?? false)
            {
                _thingRepository.Remove(currentEvent.ThingsChanges);
            }
            if (currentEvent.RequirementThings?.Any() ?? false)
            {
                _thingRepository.Remove(currentEvent.RequirementThings);
            }
            if (currentEvent.RequirementStates?.Any() ?? false)
            {
                _stateRepository.Remove(currentEvent.RequirementStates);
            }
            if (currentEvent.HeroStatesChanging?.Any() ?? false)
            {
                _stateRepository.Remove(currentEvent.HeroStatesChanging);
            }
            var heroes = _heroRepository.GetByEvent(currentEvent.Id);
            if (heroes != null)
            {
                _heroRepository.Remove(heroes);
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

        public List<Event> GetAllEventsByBook(long bookId)
        {
            return Entity.Where(x => x.Book.Id == bookId).ToList();
        }

        public List<Event> GetRootEvents(long bookId)
        {
            return Entity.Where(x => x.Book != null && x.Book.Id == bookId).ToList();
        }

        public List<Event> GetEndingEvents(long bookId)
        {
            return Entity.Where(x => x.Book != null && x.Book.Id == bookId
                                     && x.LinksFromThisEvent.Count == 0).ToList();
        }

        public bool HasChild(long eventId)
        {
            return Entity.Any(x => x.Id == eventId && x.LinksFromThisEvent.Any());
        }

        public List<Event> GetNotAvailableEvents(long bookId)
        {
            return Entity.Where(x => x.Book != null && x.Book.Id == bookId
                                     && x.LinksToThisEvent.Count == 0 && x.ForRootBook == null).ToList();
        }
    }
}