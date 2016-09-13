using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class StateTypeRepository : BaseRepository<StateType>, IStateTypeRepository
    {
        public StateTypeRepository(WriterContext db) : base(db)
        {
        }
    }
}