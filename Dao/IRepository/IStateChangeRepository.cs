﻿using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IStateChangeRepository : IBaseRepository<StateChange>
    {
        void RemoveDecision(string decision, long bookId);
    }
}