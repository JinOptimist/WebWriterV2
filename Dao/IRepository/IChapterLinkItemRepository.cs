using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IChapterLinkItemRepository : IBaseRepository<ChapterLinkItem>
    {
        List<ChapterLinkItem> GetLinksFromChapter(long chapterId);
        void RemoveDuplicates();
    }
}