using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        Event GetWithParentAndChildren(long id);

        List<Event> GetEvents(long questId);
    }
}