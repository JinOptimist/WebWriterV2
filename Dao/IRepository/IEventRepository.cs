using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        List<Event> GetByQuest(long questId);

        List<Event> GetRootEvents();
    }
}