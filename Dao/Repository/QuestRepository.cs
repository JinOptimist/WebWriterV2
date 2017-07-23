﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        private readonly EventRepository _eventRepository;
        private readonly EventLinkItemRepository _eventLinkItemRepository;
        private readonly EvaluationRepository _evaluationRepository;

        public BookRepository(WriterContext db) : base(db)
        {
            _eventRepository = new EventRepository(db);
            _eventLinkItemRepository = new EventLinkItemRepository(db);
            _evaluationRepository = new EvaluationRepository(db);
        }

        public List<Book> GetAllWithRootEvent()
        {
            return Entity.Include(x => x.RootEvent).ToList();
        }

        public List<Book> GetByUser(long userId)
        {
            return Entity.Where(x => x.Owner.Id == userId).ToList();
        }

        public override void Remove(Book book)
        {
            if (book == null)
                return;

            _evaluationRepository.Remove(book.Evaluations);

            foreach (var @event in book.AllEvents)
            {
                _eventLinkItemRepository.Remove(@event.LinksFromThisEvent);
                _eventLinkItemRepository.Remove(@event.LinksToThisEvent);
            }

            if (book.AllEvents.Count > 0)
            {
                _eventRepository.Remove(book.AllEvents);
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