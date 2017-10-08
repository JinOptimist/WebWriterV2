using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IChapterRepository : IBaseRepository<Chapter>
    {
        void RemoveEventAndChildren(Chapter currentEvent);

        void RemoveEventAndChildren(long currentEventId);

        List<Chapter> GetAllEventsByBook(long bookId);

        List<Chapter> GetRootEvents(long bookId);

        List<Chapter> GetEndingEvents(long bookId);

        List<Chapter> GetNotAvailableEvents(long bookId);

        bool HasChild(long eventId);
    }
}