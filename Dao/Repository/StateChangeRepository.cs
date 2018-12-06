using System.Linq;
using Dal.Repository;
using Dal.IRepository;
using Dal.Model;

namespace Dal.Repository
{
    public class StateChangeRepository : BaseRepository<StateChange>, IStateChangeRepository
    {
        public StateChangeRepository(WriterContext db) : base(db)
        {
        }

        public override StateChange Save(StateChange model)
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