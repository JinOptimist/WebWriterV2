﻿using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dal.Model;

namespace Dal.IRepository
{
    public interface IQuestionnaireRepository : IBaseRepository<Questionnaire>
    {
        List<Questionnaire> GetForWriter(long userId);
    }
}