using System.Linq;
using Dal.Repository;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class QuestionnaireRepository : BaseRepository<Questionnaire>, IQuestionnaireRepository
    {
        private readonly QuestionRepository _questionRepository;

        public QuestionnaireRepository(WriterContext db) : base(db)
        {
            _questionRepository = new QuestionRepository(db);
        }

        public override Questionnaire Save(Questionnaire model)
        {
            model.Questions = model.Questions.Select(x => x.AttachIfNot(Db)).ToList();

            return base.Save(model);
        }

        public override void Remove(Questionnaire baseModel)
        {
            _questionRepository.Remove(baseModel.Questions);
            base.Remove(baseModel);
        }
    }
}