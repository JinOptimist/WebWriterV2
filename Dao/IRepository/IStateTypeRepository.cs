﻿using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IStateTypeRepository : IBaseRepository<StateType>
    {
        List<StateType> AvailableForUse(long userId);

        List<StateType> AvailableForEdit(long userId);
    }
}