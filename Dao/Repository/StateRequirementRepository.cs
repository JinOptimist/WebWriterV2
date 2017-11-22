using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class StateRequirementRepository : BaseRepository<StateRequirement>, IStateRequirementRepository
    {
        public StateRequirementRepository(WriterContext db) : base(db)
        {
        }
    }
}