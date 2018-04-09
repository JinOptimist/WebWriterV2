using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User Login(string loginOrEmail, string password);

        User GetByName(string username);
    }
}