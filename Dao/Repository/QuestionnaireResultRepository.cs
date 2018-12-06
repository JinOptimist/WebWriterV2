using System.Linq;
using Dal.Repository;
using Dal.IRepository;
using Dal.Model;

namespace Dal.Repository
{
    public class QuestionnaireResultRepository : BaseRepository<QuestionnaireResult>, IQuestionnaireResultRepository
    {
        private readonly QuestionOtherAnswerRepository _questionOtherAnswerRepository;

        public QuestionnaireResultRepository(WriterContext db) : base(db)
        {
            _questionOtherAnswerRepository = new QuestionOtherAnswerRepository(db);
        }

        public override QuestionnaireResult Save(QuestionnaireResult model)
        {
            model.Questionnaire = model.Questionnaire.AttachIfNot(Db);
            model.User = model.User.AttachIfNot(Db);
            model.QuestionAnswers = model.QuestionAnswers.Select(x => x.AttachIfNot(Db)).ToList();

            var questionOtherAnswers = model.QuestionOtherAnswers;
            model.QuestionOtherAnswers = null;
            model = base.Save(model);

            questionOtherAnswers?.ForEach(x => x.QuestionnaireResult = model);
            model.QuestionOtherAnswers = _questionOtherAnswerRepository.Save(questionOtherAnswers);
            return model;
        }
    }
}