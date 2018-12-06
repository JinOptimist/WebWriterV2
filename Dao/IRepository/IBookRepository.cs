using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dal.Model;

namespace Dal.IRepository
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        List<Book> GetAllWithRootEvent();

        Book GetByName(string name);

        List<Book> GetByUser(long userId);

        List<Book> GetAll(bool getOnlyPublished);
    }
}