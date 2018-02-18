using System.Collections.Generic;
using System.Data.Entity;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        bool Exist(T baseModel);

        T Save(T baseModel);

        List<T> Save(List<T> baseModels);

        List<T> GetAll();

        T Get(long id);

        List<T> GetList(IEnumerable<long> ids);

        void Remove(long id);

        void Remove(T baseModel);

        void Remove(IEnumerable<T> models);
    }
}