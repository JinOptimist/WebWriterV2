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

        protected WriterContext Db { get; set; }// = ContextForRepository.Context;
        protected DbSet<T> Entity { get; set; }

        public virtual bool Exist(T baseModel)
        {
            return Entity.Any(x => x == baseModel);
        }

        public virtual T Save(T model)
        {
            if (model.Id > 0)
            {
                if (Db.Entry(model).State == EntityState.Detached)
                {
                    Entity.Attach(model);
                }
                Db.Entry(model).State = EntityState.Modified;
                Db.SaveChanges();
                return model;
            }

            Entity.Add(model);
            Db.SaveChanges();
            return model;
        }

        public virtual List<T> Save(List<T> baseModels)
        {
            return baseModels.Select(Save).ToList();
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

        public virtual void Remove(IEnumerable<T> models)
        {
            var copyList = models.ToList();
            foreach (var model in copyList)
            {
                Remove(model);
            }
        }
    }
}