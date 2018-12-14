using System.Linq;
using Dal.Repository;
using Dal.IRepository;
using Dal.Model;
using System.Collections.Generic;

namespace Dal.Repository
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

        public List<Questionnaire> GetForWriter(long userId)
        {
            return Entity.Where(x => x.ShowBeforeFirstBook && !x.Users.Any(u => u.Id == userId)).ToList();
        }
    }
}