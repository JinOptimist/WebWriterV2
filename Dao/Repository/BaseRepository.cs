using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
    {
        public BaseRepository(WriterContext db)
        {
            Db = db;
            Entity = Db.Set(typeof(T)).Cast<T>();
        }

        public readonly WriterContext Db;// = ContextForRepository.Context;
        public readonly DbSet<T> Entity;

        public void Save(T model)
        {
            if (model.Id > 0)
            {
                Entity.Attach(model);
                Db.Entry(model).State = EntityState.Modified;
                Db.SaveChanges();
                return;
            }

            Entity.Add(model);
            Db.SaveChanges();
        }

        public void Save(List<T> baseModels)
        {
            baseModels.ForEach(Save);
        }

        public List<T> GetAll()
        {
            return Entity.ToList();
        }

        public T Get(long id)
        {
            return Entity.FirstOrDefault(x => x.Id == id);
        }

        public void Remove(long id)
        {
            Entity.Remove(Get(id));
            Db.SaveChanges();
        }

        public void Remove(T baseModel)
        {
            if (baseModel == null)
                return;
            Entity.Remove(baseModel);
            Db.SaveChanges();
        }
    }
}