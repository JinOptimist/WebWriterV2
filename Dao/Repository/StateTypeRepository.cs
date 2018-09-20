using System;
using System.Collections.Generic;
using Dao.IRepository;
using Dao.Model;
using System.Linq;
using Dal.Repository;

namespace Dao.Repository
{
    public class StateTypeRepository : BaseRepository<StateType>, IStateTypeRepository
    {
        public StateTypeRepository(WriterContext db) : base(db)
        {
        }

        public override StateType Save(StateType model)
        {
            model.Book = model.Book.AttachIfNot(Db);
            model.Owner = model.Owner.AttachIfNot(Db);
            return base.Save(model);
        }

        public List<StateType> AvailableForUse(long userId)
        {
            return Entity.Where(x => x.Owner == null || x.Owner.Id == userId).ToList();
        }

        public List<StateType> AvailableForEdit(long userId)
        {
            return Entity.Where(x => x.Owner.Id == userId).ToList();
        }

        public StateType GetByName(string name)
        {
            return Entity.SingleOrDefault(x => x.Name == name);
        }

        public StateType GetDefault()
        {
            return Entity.Single(x => x.Name == DataGeneration.GenerateData.DefaultStateTypeName);
        }
    }
}