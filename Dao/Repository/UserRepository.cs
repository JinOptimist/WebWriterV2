using System;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(WriterContext db) : base(db)
        {
        }

        public User GetByName(string username)
        {
            return Entity.FirstOrDefault(x => x.Name == username);
        }

        public User Login(string username, string password)
        {
            return Entity.FirstOrDefault(x => x.Name == username && x.Password == password);
        }
    }
}