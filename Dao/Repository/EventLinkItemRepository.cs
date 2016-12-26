using System.Data.Entity;
using Dao.IRepository;
using Dao.Model;
using System;

namespace Dao.Repository
{
    public class EventLinkItemRepository : BaseRepository<EventLinkItem>, IEventLinkItemRepository
    {
        private readonly Lazy<EventRepository> _eventRepository;

        public EventLinkItemRepository(WriterContext db) : base(db)
        {
            _eventRepository = new Lazy<EventRepository>(() => new EventRepository(db));
        }

        public override EventLinkItem Save(EventLinkItem model)
        {
            if (_eventRepository.Value.Db.Entry(model.From).State == EntityState.Detached)
            {
                model.From = _eventRepository.Value.Entity.Find(model.From.Id);
            }

            if (_eventRepository.Value.Db.Entry(model.To).State == EntityState.Detached)
            {
                model.To = _eventRepository.Value.Entity.Find(model.To.Id);
            }

            return base.Save(model);
        }
    }
}