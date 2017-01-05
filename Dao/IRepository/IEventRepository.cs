using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        void RemoveEventAndChildren(Event currentEvent);

        void RemoveEventAndChildren(long currentEventId);

        List<Event> GetAllEventsByQuest(long questId);

        List<Event> GetRootEvents(long questId);

        List<Event> GetEndingEvents(long questId);

        List<Event> GetNotAvailableEvents(long questId);

        bool HasChild(long eventId);
    }
}