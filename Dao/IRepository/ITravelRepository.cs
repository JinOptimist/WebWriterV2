using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dal.Model;

namespace Dal.IRepository
{
    public interface ITravelRepository : IBaseRepository<Travel>
    {
        Travel GetByBookAndUser(long bookId, long userId);

        List<Travel> GetByUser(long userId);
    }
}