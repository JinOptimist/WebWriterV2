using System.Collections.Generic;
using Dal.Model;

namespace Dal.IRepository
{
    public interface IChapterRepository : IBaseRepository<Chapter>
    {
        void RemoveEventAndChildren(Chapter currentEvent);

        void RemoveEventAndChildren(long currentEventId);

        List<Chapter> GetAllChaptersByBook(long bookId);

        List<Chapter> GetRootChapter(long bookId);

        List<Chapter> GetEndingEvents(long bookId);

        List<Chapter> GetNotAvailableEvents(long bookId);

        List<Chapter> GetChapterBottom(long bookId, int level);

        List<Chapter> GetChapterTop(long bookId, int level);

        List<Chapter> GetByLevel(long bookId, int level);

        bool HasChild(long eventId);
    }
}