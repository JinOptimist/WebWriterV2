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
            Entity = Db.Set<T>();
        }

        public WriterContext Db { get; set; }// = ContextForRepository.Context;
        public DbSet<T> Entity { get; set; }

        public virtual bool Exist(T baseModel)
        {
            return Entity.Any(x => x == baseModel);
        }

        public virtual void Save(T model)
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

        public virtual void Save(List<T> baseModels)
        {
            baseModels.ForEach(Save);
        }

        public virtual List<T> GetAll()
        {
            return Entity.ToList();
        }

        public virtual T Get(long id)
        {
            return Entity.FirstOrDefault(x => x.Id == id);
        }

        public virtual List<T> GetList(IEnumerable<long> ids)
        {
            return Entity.Where(x => ids.Contains(x.Id)).ToList();
        }

        public virtual void Remove(long id)
        {
            Remove(Get(id));
        }

        public virtual void Remove(T baseModel)
        {
            if (baseModel == null)
                return;
            Entity.Remove(baseModel);
            Db.SaveChanges();
        }

        public virtual void Remove(List<T> models)
        {
            models.ForEach(Remove);
        }
    }
}