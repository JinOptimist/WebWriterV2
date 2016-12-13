using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class EventLinkItemRepository : BaseRepository<EventLinkItem>, IEventLinkItemRepository
    {
        public EventLinkItemRepository(WriterContext db) : base(db)
        {
        }
    }
}