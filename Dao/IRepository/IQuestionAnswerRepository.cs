using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dal.Model;

namespace Dal.IRepository
{
    public interface IQuestionAnswerRepository : IBaseRepository<QuestionAnswer>
    {
        List<QuestionAnswer> GetByQuestionVisibleIf(Question question);
    }
}