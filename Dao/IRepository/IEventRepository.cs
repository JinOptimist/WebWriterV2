using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        void RemoveWholeBranch(Event currentEvent);

        void RemoveWholeBranch(long currentEventId);

        void RemoveEventAndChildren(Event currentEvent);

        void RemoveEventAndChildren(long currentEventId);

        List<Event> GetAllEventsByQuest(long questId);

        List<Event> GetRootEvents(long questId);

        bool HasChild(long eventId);
    }
}