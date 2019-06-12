using System;
using System.Linq;
using Dal.IRepository;
using Dal.Model;

namespace Dal.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ITravelRepository _travelRepository;

        public UserRepository(WriterContext db) : base(db)
        {
            _travelRepository = new TravelRepository(db);
        }

        public override bool Exist(User baseModel)
        {
            return Entity.Any(x => x.Email == baseModel.Email);
        }

        public User GetByEmail(string email)
        {
            return Entity.FirstOrDefault(x => x.Email == email);
        }

        public User Login(string email, string password)
        {
            return Entity.FirstOrDefault(x => x.Email == email && x.Password == password);
        }

        public override void Remove(User user)
        {
            _travelRepository.Remove(user.MyTravels);
            base.Remove(user);
        }
    }
}