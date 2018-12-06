﻿using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dal.Model;

namespace Dal.IRepository
{
    public interface IStateRequirementRepository : IBaseRepository<StateRequirement>
    {
        void RemoveDecision(string decision, long bookId);
    }
}