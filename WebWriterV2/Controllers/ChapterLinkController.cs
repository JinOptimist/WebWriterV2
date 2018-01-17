using Dao;
using Dao.IRepository;
using Dao.Model;
using Dao.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using WebWriterV2.DI;
using WebWriterV2.FrontModels;
using WebWriterV2.VkUtility;

namespace WebWriterV2.Controllers
{
    public class ChapterLinkController : BaseApiController
    {
        public ChapterLinkController(IBookRepository bookRepository, IChapterLinkItemRepository chapterLinkItemRepository)
        {
            BookRepository = bookRepository;
            ChapterLinkItemRepository = chapterLinkItemRepository;
        }

        private IBookRepository BookRepository { get; set; }
        private IChapterLinkItemRepository ChapterLinkItemRepository { get; set; }

        [AcceptVerbs("POST")]
        public FrontChapterLinkItem Save(FrontChapterLinkItem frontChapterLinkItem)
        {
            var сhapterLinkItem = frontChapterLinkItem.ToDbModel();
            сhapterLinkItem = ChapterLinkItemRepository.Save(сhapterLinkItem);
            frontChapterLinkItem = new FrontChapterLinkItem(сhapterLinkItem);
            return frontChapterLinkItem;
        }

        [AcceptVerbs("GET")]
        public List<FrontChapterLinkItem> GetLinksFromChapter(long chapterId)
        {
            var linksFromChapter = ChapterLinkItemRepository.GetLinksFromChapter(chapterId);
            return linksFromChapter.Select(x => new FrontChapterLinkItem(x)).ToList();
        }

        [AcceptVerbs("GET")]
        public bool Remove(long id)
        {
            ChapterLinkItemRepository.Remove(id);
            return true;
        }
    }
}
