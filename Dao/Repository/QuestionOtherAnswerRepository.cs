using System.Linq;
using Dal.IRepository;
using Dal.Model;

namespace Dal.Repository
{
    public class QuestionOtherAnswerRepository : BaseRepository<QuestionOtherAnswer>, IQuestionOtherAnswerRepository
    {
        public QuestionOtherAnswerRepository(WriterContext db) : base(db)
        {
        }

        public override QuestionOtherAnswer Save(QuestionOtherAnswer model)
        {
            model.Question = model.Question.AttachIfNot(Db);
            model.QuestionnaireResult = model.QuestionnaireResult.AttachIfNot(Db);
            return base.Save(model);
        }
    }
}