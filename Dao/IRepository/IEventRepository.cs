using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        Event GetWithParentAndChildren(long id);

        Event GetWithChildren(long id);

        List<Event> GetEventsWithChildren(long questId);

        List<Event> GetRootEvents();
    }
}