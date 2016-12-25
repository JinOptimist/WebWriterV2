using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class ThingRepository : BaseRepository<Thing>, IThingRepository
    {
        public ThingRepository(WriterContext db) : base(db)
        {
        }
    }
}