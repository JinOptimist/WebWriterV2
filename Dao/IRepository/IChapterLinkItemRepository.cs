using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dal.Model;

namespace Dal.IRepository
{
    public interface IChapterLinkItemRepository : IBaseRepository<ChapterLinkItem>
    {
        List<ChapterLinkItem> GetLinksFromChapter(long chapterId);
        void RemoveDuplicates();

        bool Exist(long fromId, long toId);
    }
}