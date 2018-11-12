using System.Linq;
using Dal.Repository;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(WriterContext db) : base(db)
        {
        }

        public override Question Save(Question model)
        {
            model.Questionnaire = model.Questionnaire.AttachIfNot(Db);
            return base.Save(model);
        }
    }
}