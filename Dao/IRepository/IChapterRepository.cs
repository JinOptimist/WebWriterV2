﻿using System.Collections.Generic;
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

        List<Chapter> GetChapterBottom(long bookId, int level);

        List<Chapter> GetChapterTop(long bookId, int level);

        bool HasChild(long eventId);
    }
}