using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        void RemoveEventAndChildren(Event currentEvent);

        void RemoveEventAndChildren(long currentEventId);

        List<Event> GetAllEventsByBook(long bookId);

        List<Event> GetRootEvents(long bookId);

        List<Event> GetEndingEvents(long bookId);

        List<Event> GetNotAvailableEvents(long bookId);

        bool HasChild(long eventId);
    }
}