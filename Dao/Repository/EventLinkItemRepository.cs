using System.Data.Entity;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class EventLinkItemRepository : BaseRepository<EventLinkItem>, IEventLinkItemRepository
    {
        private readonly EventRepository _eventRepository;

        public EventLinkItemRepository(WriterContext db) : base(db)
        {
            _eventRepository= new EventRepository(db);
        }

        public override EventLinkItem Save(EventLinkItem model)
        {
            if (_eventRepository.Db.Entry(model.From).State == EntityState.Detached)
            {
                model.From = _eventRepository.Entity.Find(model.From.Id);
            }

            if (_eventRepository.Db.Entry(model.To).State == EntityState.Detached)
            {
                model.To = _eventRepository.Entity.Find(model.To.Id);
            }

            return base.Save(model);
        }
    }
}