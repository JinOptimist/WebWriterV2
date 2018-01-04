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
    public class ChapterController : BaseApiController
    {
        public ChapterController(IBookRepository bookRepository, IChapterRepository chapterRepository)
        {
            BookRepository = bookRepository;
            ChapterRepository = chapterRepository;
        }

        private IBookRepository BookRepository { get; set; }
        private IChapterRepository ChapterRepository { get; set; }
        
        [AcceptVerbs("POST")]
        public FrontChapter Save(FrontChapter frontChapter)
        {
            var chapter = frontChapter.ToDbModel();
            chapter = ChapterRepository.Save(chapter);
            frontChapter = new FrontChapter(chapter);
            return frontChapter;
        }

        [AcceptVerbs("GET")]
        public FrontChapter Get(long id)
        {
            var chapter = ChapterRepository.Get(id);
            var frontChapter = new FrontChapter(chapter);
            return frontChapter;
        }
    }
}
