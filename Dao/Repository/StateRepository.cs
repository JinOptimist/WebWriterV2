using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class StateRepository : BaseRepository<State>, IStateRepository
    {
        public StateRepository(WriterContext db) : base(db)
        {
        }
    }
}