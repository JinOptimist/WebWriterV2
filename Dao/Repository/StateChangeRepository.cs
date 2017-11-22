using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class StateChangeRepository : BaseRepository<StateChange>, IStateChangeRepository
    {
        public StateChangeRepository(WriterContext db) : base(db)
        {
        }
    }
}