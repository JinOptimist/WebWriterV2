using System.Linq;
using Dal.Repository;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class StateRequirementRepository : BaseRepository<StateRequirement>, IStateRequirementRepository
    {
        public StateRequirementRepository(WriterContext db) : base(db)
        {
        }

        public override StateRequirement Save(StateRequirement model)
        {
            model.StateType = model.StateType.AttachIfNot(Db);
            model.ChapterLink = model.ChapterLink.AttachIfNot(Db);

            return base.Save(model);
        }

        public void RemoveDecision(string decision, long bookId)
        {
            var conditions = Entity.Where(x => x.ChapterLink.From.Book.Id == bookId && x.Text == decision);
            Remove(conditions);
        }
    }
}