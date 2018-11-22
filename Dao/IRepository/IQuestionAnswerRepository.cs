using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IQuestionAnswerRepository : IBaseRepository<QuestionAnswer>
    {
        List<QuestionAnswer> GetByQuestionVisibleIf(Question question);
    }
}