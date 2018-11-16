using System.Linq;
using Dal.Repository;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class QuestionnaireResultRepository : BaseRepository<QuestionnaireResult>, IQuestionnaireResultRepository
    {
        public QuestionnaireResultRepository(WriterContext db) : base(db)
        {
        }

        public override QuestionnaireResult Save(QuestionnaireResult model)
        {
            model.Questionnaire = model.Questionnaire.AttachIfNot(Db);
            model.User = model.User.AttachIfNot(Db);
            model.QuestionAnswers = model.QuestionAnswers.Select(x => x.AttachIfNot(Db)).ToList();
            return base.Save(model);
        }
    }
}