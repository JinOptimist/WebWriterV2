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

        public override bool Exist(User baseModel)
        {
            return Entity.Any(x => x.Name == baseModel.Name || x.Email == baseModel.Email);
        }

        public User GetByName(string username)
        {
            return Entity.FirstOrDefault(x => x.Name == username);
        }

        public User Login(string loginOrEmail, string password)
        {
            return Entity.FirstOrDefault(x => (x.Name == loginOrEmail || x.Email == loginOrEmail) && x.Password == password);
        }
    }
}