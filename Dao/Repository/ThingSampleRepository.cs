using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class ThingSampleRepository : BaseRepository<ThingSample>, IThingSampleRepository
    {
        public ThingSampleRepository(WriterContext db) : base(db)
        {
        }
    }
}