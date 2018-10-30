using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        private readonly ChapterRepository _eventRepository;
        private readonly ChapterLinkItemRepository _eventLinkItemRepository;
        private readonly EvaluationRepository _evaluationRepository;
        private readonly TravelRepository _travelRepository;
        private readonly StateTypeRepository _stateTypeRepository;
        

        public BookRepository(WriterContext db) : base(db)
        {
            _eventRepository = new ChapterRepository(db);
            _eventLinkItemRepository = new ChapterLinkItemRepository(db);
            _evaluationRepository = new EvaluationRepository(db);
            _travelRepository = new TravelRepository(db);

            _stateTypeRepository = new StateTypeRepository(db);
        }

        public List<Book> GetAllWithRootEvent()
        {
            return Entity.Include(x => x.RootChapter).ToList();
        }

        public List<Book> GetByUser(long userId)
        {
            return Entity.Where(x => x.Owner.Id == userId).ToList();
        }

        public override void Remove(Book book)
        {
            if (book == null){
                return;
            }

            _travelRepository.Remove(book.Travels);
            _evaluationRepository.Remove(book.Evaluations);

            foreach (var @event in book.AllChapters)
            {
                _eventLinkItemRepository.Remove(@event.LinksFromThisChapter);
                _eventLinkItemRepository.Remove(@event.LinksToThisChapter);
            }

            if (book.AllChapters.Count > 0)
            {
                _eventRepository.Remove(book.AllChapters);
            }

            if (book.States.Any()) {
                _stateTypeRepository.Remove(book.States);
            }

            book = Get(book.Id);
            base.Remove(book);
        }

        /// <summary>
        /// If we update model, we update only Name and Desc field. Don't forget this
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override Book Save(Book model)
        {
            // if we try update detached model
            if (Db.Entry(model).State == EntityState.Detached && model.Id > 0)
            {
                var modelFromDb = Get(model.Id);
                modelFromDb.UpdateFrom(model);
                model = modelFromDb;
            }

            if (Db.Entry(model.Owner).State == EntityState.Detached) {
                Db.Set<User>().Attach(model.Owner);
            }

            return base.Save(model);
        }

        public Book GetByName(string name)
        {
            return Entity.FirstOrDefault(x => x.Name == name);
        }

        public override List<Book> GetAll()
        {
            throw new Exception("Don't use default GetAll() in BookRepository, instead of this use GetAll(bool getOnlyPublished)");
        }

        public List<Book> GetAll(bool getOnlyPublished)
        {
            if (getOnlyPublished) {
                return Entity.Where(x => x.IsPublished == true).ToList();
            }

            return base.GetAll();
        }
    }
}