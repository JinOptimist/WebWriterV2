using Dal.IRepository;
using Dal.Model;

namespace Dal.Repository
{
    public class EvaluationRepository : BaseRepository<Evaluation>, IEvaluationRepository
    {
        public EvaluationRepository(WriterContext db) : base(db)
        {
        }
    }
}