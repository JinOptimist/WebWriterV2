using System.Linq;
using Dal.Repository;
using Dal.IRepository;
using Dal.Model;

namespace Dal.Repository
{
    public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
    {
        private readonly QuestionAnswerRepository _questionAnswerRepository;

        public QuestionRepository(WriterContext db) : base(db)
        {
            _questionAnswerRepository = new QuestionAnswerRepository(db);
        }

        public override Question Save(Question model)
        {
            model.Questionnaire = model.Questionnaire.AttachIfNot(Db);
            model.Answers = model.Answers.Select(x => x.AttachIfNot(Db)).ToList();
            model.VisibleIf = model.VisibleIf.Select(x => x.AttachIfNot(Db)).ToList();
            return base.Save(model);
        }

        public override void Remove(Question baseModel)
        {
            _questionAnswerRepository.Remove(baseModel.Answers);
            base.Remove(baseModel);
        }
    }
}