using System.Linq;
using Dal.Repository;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class QuestionnaireRepository : BaseRepository<Questionnaire>, IQuestionnaireRepository
    {
        public QuestionnaireRepository(WriterContext db) : base(db)
        {
        }

        public override Questionnaire Save(Questionnaire model)
        {
            model.Questions = model.Questions.Select(x => x.AttachIfNot(Db)).ToList();

            return base.Save(model);
        }
    }
}