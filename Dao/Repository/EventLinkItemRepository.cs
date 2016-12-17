using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class EventLinkItemRepository : BaseRepository<EventLinkItem>, IEventLinkItemRepository
    {
        public EventLinkItemRepository(WriterContext db) : base(db)
        {
        }

        public override EventLinkItem Save(EventLinkItem model)
        {
            model.From = new Event {Id = model.From.Id};
            model.To = new Event {Id = model.To.Id};
            return base.Save(model);
        }
    }
}