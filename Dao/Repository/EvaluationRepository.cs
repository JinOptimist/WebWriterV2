using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class EvaluationRepository : BaseRepository<Evaluation>, IEvaluationRepository
    {
        public EvaluationRepository(WriterContext db) : base(db)
        {
        }
    }
}