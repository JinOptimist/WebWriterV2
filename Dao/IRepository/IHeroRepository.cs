using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IHeroRepository : IBaseRepository<Hero>
    {
        List<Hero> GetByEvent(long eventId);

        void RemoveByEvent(long eventId, long userId);

        void RemoveByQuest(long questId, long userId);
    }
}