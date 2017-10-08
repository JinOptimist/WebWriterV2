using System.Data.Entity;
using Dao.IRepository;
using Dao.Model;
using System;
using System.Linq;

namespace Dao.Repository
{
    public class EventLinkItemRepository : BaseRepository<EventLinkItem>, IEventLinkItemRepository
    {
        private readonly Lazy<ChapterRepository> _eventRepository;

        public EventLinkItemRepository(WriterContext db) : base(db)
        {
            _eventRepository = new Lazy<ChapterRepository>(() => new ChapterRepository(db));
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

        public void RemoveDuplicates()
        {
            var duplicate = Entity.Where(x => true)
                .GroupBy(x => new {x.From, x.Text, x.To})
                .SelectMany(x => x.OrderBy(y => y.Id).Skip(1));
            Remove(duplicate);
        }
    }
}