using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        List<Book> GetAllWithRootEvent();

        Book GetByName(string name);

        List<Book> GetByUser(long userId);
    }
}