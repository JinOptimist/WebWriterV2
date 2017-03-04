using System;
using System.Collections.Generic;
using Dao.IRepository;
using Dao.Model;
using System.Linq;

namespace Dao.Repository
{
    public class StateTypeRepository : BaseRepository<StateType>, IStateTypeRepository
    {
        public StateTypeRepository(WriterContext db) : base(db)
        {
        }

        public List<StateType> GetForUser(long userId)
        {
            return Entity.Where(x => x.Owner == null || x.Owner.Id == userId).ToList();
        }
    }
}