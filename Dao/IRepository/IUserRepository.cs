using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User Login(string username, string password);

        User GetByName(string username);
    }
}