﻿using Dao;
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
            //Update root chapter
            if (frontChapter.IsRootChapter) {
                var book = BookRepository.Get(frontChapter.BookId);
                chapter.Book = book;
                chapter = ChapterRepository.Save(chapter);
                chapter.Book.RootChapter = chapter;
                BookRepository.Save(book);
            } else {
                chapter = ChapterRepository.Save(chapter);
            }
            
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

        [AcceptVerbs("GET")]
        public FrontChapter GetForTravel(long id)
        {
            var chapter = ChapterRepository.Get(id);
            var frontChapter = new FrontChapter(chapter, true);
            return frontChapter;
        }

        [AcceptVerbs("GET")]
        public bool Remove(long id)
        {
            ChapterRepository.Remove(id);
            return true;
        }

        [AcceptVerbs("POST")]
        public List<FrontChapter> GetChapterBottom(FrontChapter chapter)
        {
            var chapters = ChapterRepository.GetChapterBottom(chapter.BookId, chapter.Level);
            return chapters.Select(x=> new FrontChapter(x)).ToList();
        }

        [AcceptVerbs("POST")]
        public List<FrontChapter> GetChapterTop(FrontChapter chapter)
        {
            var chapters = ChapterRepository.GetChapterTop(chapter.BookId, chapter.Level);
            return chapters.Select(x => new FrontChapter(x)).ToList();
        }
    }
}
