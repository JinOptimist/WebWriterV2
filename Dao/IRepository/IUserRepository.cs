using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dal.Model;

namespace Dal.IRepository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User Login(string loginOrEmail, string password);

        User GetByName(string username);
    }
}