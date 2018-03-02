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

        public void RemoveDecision(string decision, long bookId)
        {
            var conditions = Entity.Where(x => x.ChapterLink.From.Book.Id == bookId && x.Text == decision);
            Remove(conditions);
        }
    }
}