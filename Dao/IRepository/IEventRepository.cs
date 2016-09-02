using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        Event GetWithParentAndChildren(long id);

        void RemoveEventAndHisChildren(Event currentEvent);

        void RemoveEventAndHisChildren(long id);

        List<Event> GetEvents(long questId);
    }
}