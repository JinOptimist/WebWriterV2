using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dao.Model;

namespace Dao.IRepository
{
    public interface ITravelRepository : IBaseRepository<Travel>
    {
        Travel GetByBookAndUser(long bookId, long userId);

        List<Travel> GetByUser(long userId);
    }
}