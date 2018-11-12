using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class QuestionnaireResultRepository : BaseRepository<QuestionnaireResult>, IQuestionnaireResultRepository
    {
        public QuestionnaireResultRepository(WriterContext db) : base(db)
        {
        }
    }
}