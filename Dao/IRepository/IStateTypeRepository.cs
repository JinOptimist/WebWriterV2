﻿using System.Collections.Generic;
using Dal.Model;

namespace Dal.IRepository
{
    public interface IStateTypeRepository : IBaseRepository<StateType>
    {
        List<StateType> AvailableForUse(long userId);

        List<StateType> AvailableForEdit(long userId);

        StateType GetByName(string name);

        StateType GetDefault();
    }
}