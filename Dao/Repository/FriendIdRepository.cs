using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class FriendIdRepository : IFriendIdRepository
    {
        private readonly WriterContext _db = new WriterContext();

        public int CountFriendId()
        {
            return _db.FriendId.Count();
        }
    }
}