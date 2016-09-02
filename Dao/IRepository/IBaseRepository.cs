using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IBaseRepository<T>
    {
        void Save(T baseModel);

        List<T> GetAll();

        T Get(long id);

        void Remove(long id);

        void Remove(T baseModel);
    }
}