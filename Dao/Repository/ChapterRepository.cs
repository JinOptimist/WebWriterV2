using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class ChapterRepository : BaseRepository<Chapter>, IChapterRepository
    {
        private readonly Lazy<ChapterLinkItemRepository> _eventLinkItemRepository;
        private readonly ThingRepository _thingRepository;
        private readonly StateValueRepository _stateValueRepository;
        private readonly HeroRepository _heroRepository;

        public const string RemoveExceptionMessage = "If you want remove event wich has children use method RemoveWholeBranch or RemoveEventAndChildren";
        public ChapterRepository(WriterContext db) : base(db)
        {
            _eventLinkItemRepository = new Lazy<ChapterLinkItemRepository>(() => new ChapterLinkItemRepository(db));
            _thingRepository = new ThingRepository(db);
            _stateValueRepository = new StateValueRepository(db);
            _heroRepository = new HeroRepository(db);
        }

        public override Chapter Save(Chapter model)
        {
            if (model.NumberOfWords == 0 && model.Desc.Length > 0) {
                throw new Exception("You foget fill NumberOfWords before save");
            }

            // if we try update detached model
            var entry = Db.Entry(model);
            if (entry.State == EntityState.Detached && model.Id > 0)
            {
                var modelFromDb = Entity.Find(model.Id);
                modelFromDb.UpdateFrom(model);
                model = modelFromDb;
            }

            if (model.Book.RootChapter == null)
            {
                model.Book.RootChapter = model;
            }

            return base.Save(model);
        }

        public override void Remove(Chapter currentEvent)
        {
            if (currentEvent == null)
                return;

            if (currentEvent.LinksFromThisChapter?.Any() ?? false)
            {
                _eventLinkItemRepository.Value.Remove(currentEvent.LinksFromThisChapter);
            }
            if (currentEvent.LinksToThisChapter?.Any() ?? false)
            {
                _eventLinkItemRepository.Value.Remove(currentEvent.LinksToThisChapter);
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

        public void RemoveEventAndChildren(Chapter currentEvent)
        {
            if (currentEvent == null)
                return;

            if (currentEvent.LinksFromThisChapter?.Any() ?? false)
            {
                var forDelete = currentEvent.LinksFromThisChapter.Select(x => x.From).ToList();
                _eventLinkItemRepository.Value.Remove(currentEvent.LinksFromThisChapter);
                forDelete.ForEach(RemoveEventAndChildren);
            }

            if (currentEvent.LinksToThisChapter.Any())
            {
                _eventLinkItemRepository.Value.Remove(currentEvent.LinksToThisChapter);
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

        public List<Chapter> GetAllEventsByBook(long bookId)
        {
            return Entity.Where(x => x.Book.Id == bookId).ToList();
        }

        public List<Chapter> GetRootEvents(long bookId)
        {
            return Entity.Where(x => x.Book != null && x.Book.Id == bookId).ToList();
        }

        public List<Chapter> GetEndingEvents(long bookId)
        {
            return Entity.Where(x => x.Book != null && x.Book.Id == bookId
                                     && x.LinksFromThisChapter.Count == 0).ToList();
        }

        public bool HasChild(long eventId)
        {
            return Entity.Any(x => x.Id == eventId && x.LinksFromThisChapter.Any());
        }

        public List<Chapter> GetNotAvailableEvents(long bookId)
        {
            return Entity.Where(x => x.Book != null && x.Book.Id == bookId
                                     && x.LinksToThisChapter.Count == 0 && x.ForRootBook == null).ToList();
        }
    }
}