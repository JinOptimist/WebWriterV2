﻿using Dal;
using Dal.IRepository;
using Dal.Model;
using Dal.Repository;
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
        public ChapterController(IBookRepository bookRepository, IChapterRepository chapterRepository, IChapterLinkItemRepository chapterLinkItemRepository, IStateTypeRepository stateTypeRepository)
        {
            BookRepository = bookRepository;
            ChapterRepository = chapterRepository;
            ChapterLinkItemRepository = chapterLinkItemRepository;
            StateTypeRepository = stateTypeRepository;
        }

        private IBookRepository BookRepository { get; set; }
        private IChapterRepository ChapterRepository { get; set; }
        private IChapterLinkItemRepository ChapterLinkItemRepository { get; set; }
        private IStateTypeRepository StateTypeRepository { get; set; }

        [AcceptVerbs("POST")]
        public FrontChapter Save(FrontChapter frontChapter)
        {
            var chapter = frontChapter.ToDbModel();
            //Update root chapter
            if (frontChapter.IsRootChapter)
            {
                var book = BookRepository.Get(frontChapter.BookId);
                chapter.Book = book;
                chapter = ChapterRepository.Save(chapter);
                chapter.Book.RootChapter = chapter;
                BookRepository.Save(book);
            }
            else
            {
                var parentsForNewChapter = new List<Chapter>();
                // New chapter must be linked with parrent.
                if (chapter.Id == 0 && chapter.Level > 1)
                {
                    parentsForNewChapter = ChapterRepository.GetByLevel(chapter.Book.Id, chapter.Level - 1);
                }

                chapter = ChapterRepository.Save(chapter);

                // New chapter must be linked with parrent.
                foreach (var parent in parentsForNewChapter)
                {
                    var link = new ChapterLinkItem()
                    {
                        From = parent,
                        To = chapter
                    };
                    ChapterLinkItemRepository.Save(link);
                }
            }

            frontChapter = new FrontChapter(chapter);
            return frontChapter;
        }

        [AcceptVerbs("GET")]
        public FrontChapter CreateChild(long parentId)
        {
            var parent = ChapterRepository.Get(parentId);

            var child = new Chapter
            {
                Name = "Глава. Ур: " + parent.Level + 1,
                Desc = "Глава создана из главы: " + parent.Name,
                Book = parent.Book,
                Level = parent.Level + 1,
                NumberOfWords = 1
            };
            child = ChapterRepository.Save(child);

            // New chapter must be linked with parrent.
            var link = new ChapterLinkItem()
            {
                From = parent,
                To = child
            };
            ChapterLinkItemRepository.Save(link);

            var frontChapter = new FrontChapter(child);
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
            var emptyTavel = new Travel();
            var frontChapter = new FrontChapter(chapter, emptyTavel);
            return frontChapter;
        }

        [AcceptVerbs("GET")]
        public bool Remove(long id)
        {
            var chapter = ChapterRepository.Get(id);

            var existForbidenLink = chapter.LinksFromThisChapter.Any(link => {
                if (link.To.LinksToThisChapter.Count > 1) {
                    return false;
                }

                var ch = link.To.LinksToThisChapter.Single().To;
                if (ch.ForRootBook == null) {
                    return true;
                }

                return false;
            });

            if (chapter.ForRootBook != null || existForbidenLink)
            {
                // if curent chapter is root chapter
                // OR there is at least one child who has only one parent 
                // we shouldn't remove chapter because the action create isolated chapter

                return false;
            }

            ChapterRepository.Remove(chapter);
            return true;
        }

        [AcceptVerbs("POST")]
        public List<FrontChapter> GetChapterBottom(FrontChapter chapter)
        {
            var chapters = ChapterRepository.GetChapterBottom(chapter.BookId, chapter.Level);
            return chapters.Select(x => new FrontChapter(x)).ToList();
        }

        [AcceptVerbs("POST")]
        public List<FrontChapter> GetChapterTop(FrontChapter chapter)
        {
            var chapters = ChapterRepository.GetChapterTop(chapter.BookId, chapter.Level);
            return chapters.Select(x => new FrontChapter(x)).ToList();
        }

        [AcceptVerbs("GET")]
        public List<FrontChapter> GetAllChapters(long bookId)
        {
            var chapters = ChapterRepository.GetAllChaptersByBook(bookId);
            return chapters.Select(x => new FrontChapter(x)).ToList();
        }

        [AcceptVerbs("POST")]
        public FrontChapter CreateNextChapter(FrontChapter frontChapter)
        {
            var chapter = frontChapter.ToDbModel();
            var newChapter = new Chapter()
            {
                Name = "Name",
                Desc = "Desc",
                Book = chapter.Book,
                Level = chapter.Level + 1,
                NumberOfWords = 1,
            };

            newChapter = ChapterRepository.Save(newChapter);
            var link = new ChapterLinkItem()
            {
                From = chapter,
                To = newChapter
            };

            ChapterLinkItemRepository.Save(link);
            return new FrontChapter(newChapter);
        }
    }
}
