using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dal.Repository;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class ChapterRepository : BaseRepository<Chapter>, IChapterRepository
    {
        private readonly Lazy<ChapterLinkItemRepository> _eventLinkItemRepository;
        private readonly Lazy<TravelRepository> _travelRepository;
        private readonly StateValueRepository _stateValueRepository;

        public const string RemoveExceptionMessage = "If you want remove event wich has children use method RemoveWholeBranch or RemoveEventAndChildren";
        public ChapterRepository(WriterContext db) : base(db)
        {
            _eventLinkItemRepository = new Lazy<ChapterLinkItemRepository>(() => new ChapterLinkItemRepository(db));
            _travelRepository = new Lazy<TravelRepository>(() => new TravelRepository(db));
            _stateValueRepository = new StateValueRepository(db);
        }

        public override Chapter Save(Chapter model)
        {
            if (model.NumberOfWords == 0 && model.Desc.Length > 0) {
                throw new Exception("You foget fill NumberOfWords before save");
            }

            // if we try update detached model
            //var entry = Db.Entry(model);
            //if (entry.State == EntityState.Detached && model.Id > 0) {
            //    var modelFromDb = Entity.Find(model.Id);
            //    modelFromDb.UpdateFrom(model);
            //    model = modelFromDb;
            //}

            model.Book = model.Book.AttachIfNot(Db);

            for (var i = 0; model.LinksFromThisChapter != null && i < model.LinksFromThisChapter.Count; i++) {
                model.LinksFromThisChapter[i] = model.LinksFromThisChapter[i].AttachIfNot(Db);
            }
            for (var i = 0; model.LinksToThisChapter != null && i < model.LinksToThisChapter.Count; i++) {
                model.LinksToThisChapter[i] = model.LinksToThisChapter[i].AttachIfNot(Db);
            }

            return base.Save(model);
        }

        public override void Remove(Chapter currentChapter)
        {
            if (currentChapter == null)
                return;

            if (currentChapter.LinksFromThisChapter?.Any() ?? false) {
                _eventLinkItemRepository.Value.Remove(currentChapter.LinksFromThisChapter);
            }
            if (currentChapter.LinksToThisChapter?.Any() ?? false) {
                _eventLinkItemRepository.Value.Remove(currentChapter.LinksToThisChapter);
            }
            _travelRepository.Value.Remove(currentChapter.Book.Travels);

            base.Remove(currentChapter);
        }

        public void RemoveEventAndChildren(long currentEventId)
        {
            RemoveEventAndChildren(Get(currentEventId));
        }

        public void RemoveEventAndChildren(Chapter currentEvent)
        {
            if (currentEvent == null)
                return;

            if (currentEvent.LinksFromThisChapter?.Any() ?? false) {
                var forDelete = currentEvent.LinksFromThisChapter.Select(x => x.From).ToList();
                _eventLinkItemRepository.Value.Remove(currentEvent.LinksFromThisChapter);
                forDelete.ForEach(RemoveEventAndChildren);
            }

            if (currentEvent.LinksToThisChapter.Any()) {
                _eventLinkItemRepository.Value.Remove(currentEvent.LinksToThisChapter);
            }

            var isDetached = Db.Entry(currentEvent).State == EntityState.Detached;
            if (isDetached) {
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

        public List<Chapter> GetAllChaptersByBook(long bookId)
        {
            return Entity.Where(x => x.Book.Id == bookId).ToList();
        }

        public List<Chapter> GetRootChapter(long bookId)
        {
            return Entity.Where(x => x.Book != null && x.Book.Id == bookId).ToList();
        }

        public List<Chapter> GetChapterBottom(long bookId, int level)
        {
            return Entity.Where(x => x.Book.Id == bookId && x.Level > level).ToList();
        }

        public List<Chapter> GetChapterTop(long bookId, int level)
        {
            return Entity.Where(x => x.Book.Id == bookId && x.Level < level).ToList();
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

        public List<Chapter> GetByLevel(long bookId, int level)
        {
            return Entity.Where(x => x.Book.Id == bookId && x.Level == level).ToList();
        }
    }
}