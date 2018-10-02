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
using WebWriterV2.RpgUtility;
using WebWriterV2.VkUtility;

namespace WebWriterV2.Controllers
{
    public class BookController : BaseApiController
    {
        public BookController(IBookRepository bookRepository, IChapterRepository chapterRepository, IEvaluationRepository evaluationRepository, IChapterLinkItemRepository eventLinkItemRepository, IStateValueRepository stateValueRepository, IStateTypeRepository stateTypeRepository, ITagRepository tagRepository, IUserRepository userRepository)
        {
            BookRepository = bookRepository;
            ChapterRepository = chapterRepository;
            EvaluationRepository = evaluationRepository;
            EventLinkItemRepository = eventLinkItemRepository;
            StateValueRepository = stateValueRepository;
            StateTypeRepository = stateTypeRepository;
            TagRepository = tagRepository;
            UserRepository = userRepository;
        }

        private IBookRepository BookRepository { get; }
        private IChapterRepository ChapterRepository { get; }
        private IEvaluationRepository EvaluationRepository { get; }
        private IChapterLinkItemRepository EventLinkItemRepository { get; }
        private IStateValueRepository StateValueRepository { get; }
        private IStateTypeRepository StateTypeRepository { get; }
        private ITagRepository TagRepository { get; }
        private IUserRepository UserRepository { get; }

        [AcceptVerbs("GET")]
        public List<FrontBook> GetAll()
        {
            var books = BookRepository.GetAll(true);
            // Guest can't read books with Statment
            // Remove for guest books with Statment
            books = books.Where(book => User != null 
                    || !book.AllChapters
                        .SelectMany(chapter => chapter.LinksFromThisChapter)
                        .Any(x => x.StateRequirement.Any())).ToList();

            var frontBooks = books.Select(x => new FrontBook(x, user: User)).ToList();
            return frontBooks;
        }

        [AcceptVerbs("GET")]
        public List<FrontBook> GetAllForWriter()
        {
            var getOnlyPublished = User == null || User.UserType != UserType.Admin;
            var books = BookRepository.GetByUser(User.Id);

            var frontBooks = books.Select(x => new FrontBook(x)).ToList();
            return frontBooks;
        }

        [AcceptVerbs("POST")]
        public FrontBook Save(FrontBook frontBook)
        {
            var book = frontBook.ToDbModel();
            book.Owner = User;
            book.Desc = book.Desc ?? book.Name;
            book = BookRepository.Save(book);

            if (!book.AllChapters?.Any() ?? true) {
                var rootChapter = new Chapter {
                    Name = "Начальная глава",
                    Desc = "Начало",
                    NumberOfWords = 1,
                    Book = book,
                    ForRootBook = book
                };
                ChapterRepository.Save(rootChapter);
            }

            frontBook = new FrontBook(book, true);
            return frontBook;
        }

        [AcceptVerbs("GET")]
        public FrontBook Get(long id)
        {
            var book = BookRepository.Get(id);
            var frontBook = new FrontBook(book, true);
            return frontBook;
        }

        [AcceptVerbs("GET")]
        public FrontBookWithChapters GetWithChapters(long id)
        {
            var book = BookRepository.Get(id);
            var frontBook = new FrontBookWithChapters(book, true);
            return frontBook;
        }

        [AcceptVerbs("GET")]
        public bool PublishBook(long bookId, bool newValue)
        {
            var book = BookRepository.Get(bookId);
            book.IsPublished = newValue;
            book.PublicationDate = DateTime.Now;
            BookRepository.Save(book);
            return true;
        }

        [AcceptVerbs("GET")]
        public bool Remove(long id)
        {
            BookRepository.Remove(id);
            return true;
        }

        [AcceptVerbs("GET")]
        public FrontTag AddTag(string tagName, long bookId)
        {
            var tag = TagRepository.GetOrCreate(tagName);
            var book = BookRepository.Get(bookId);
            if (!book.Tags.Any(x => x.Id == tag.Id)) {
                book.Tags.Add(tag);
                BookRepository.Save(book);
            }

            var frontTag = new FrontTag(tag);
            return frontTag;
        }

        [AcceptVerbs("GET")]
        public bool RemoveTag(long bookId, long tagId)
        {
            var tag = TagRepository.Get(tagId);
            var book = BookRepository.Get(bookId);
            book.Tags.Remove(tag);
            BookRepository.Save(book);

            return true;
        }

        [AcceptVerbs("GET")]
        public List<FrontBook> GetAllForAdmin()
        {
            if (User.UserType != UserType.Admin)
            {
                throw new Exception("Как ты узнал об этом урле? -_-");
            }

            var books = BookRepository.GetAll(false);
            var frontBooks = books.Select(x => new FrontBook(x)).ToList();

            return frontBooks;
        }

        [AcceptVerbs("GET")]
        public BookWithChaptersV2 GetWithChaptersV2(long id)
        {
            var book = BookRepository.Get(id);
            var frontBook = new BookWithChaptersV2(book);
            return frontBook;
        }

        [AcceptVerbs("POST")]
        public FrontStateType AddState(FrontStateType fontStateType)
        {
            var stateType = fontStateType.ToDbModel();

            stateType = StateTypeRepository.Save(stateType);
            fontStateType = new FrontStateType(stateType);
            return fontStateType;
        }

        [AcceptVerbs("GET")]
        public bool RemoveState(long stateTypeId)
        {
            StateTypeRepository.Remove(stateTypeId);
            return true;
        }

        [AcceptVerbs("GET")]
        public int AddView(long bookId)
        {
            var book = BookRepository.Get(bookId);
            book.Views++;
            BookRepository.Save(book);
            return book.Views;
        }

        //EXPEREMENAL
        [AcceptVerbs("GET")]
        public FrontBookWithChapters GetWithChaptersRoadmap(long id)
        {
            var book = BookRepository.Get(id);
            var frontBook = new FrontBookWithChapters(book, true, true);
            return frontBook;
        }


		//Just for test
        [AcceptVerbs("GET")]
        public List<FrontChapter> StatisticOfVisiting(long bookId)
        {
            var book = BookRepository.Get(bookId);
            var elChel = new ElChel(book);
            return elChel.StatisticOfVisitingAllWay();
        }


        [AcceptVerbs("GET")]
        public string ClearFakeStateType()
        {
            var states = StateTypeRepository.GetAll().Where(x => x.BasicType == 0);
            var count = states.Count();
            StateTypeRepository.Remove(states);
            return $"Good job. I remove {count} states";
        }



        // old GetBooks(long? userId) with userId
        public List<FrontBook> GetByUser(long id)
        {
            var books = BookRepository.GetByUser(id);
            var frontBooks = books.Select(x => new FrontBook(x)).ToList();
            return frontBooks;
        }

        public long ImportBook(string jsonBook)
        {
            var frontBook = SerializeHelper.Deserialize<FrontBook>(jsonBook);
            var book = frontBook.ToDbModel();

            var bookName = BookRepository.GetByName(book.Name);
            if (bookName == null) {
                book.Id = 0;
                book.Owner = User;
                var states = new List<StateValue>();
                var linkItems = new List<ChapterLinkItem>();

                foreach (var chapter in book.AllChapters) {
                    chapter.Id = 0;
                    if (book.RootChapter.Id == chapter.Id) {
                        chapter.ForRootBook = book;
                    }

                    var chapterLinkItems = chapter.LinksFromThisChapter;
                    chapterLinkItems.AddRange(chapter.LinksToThisChapter);
                    foreach (var chapterLinkItem in chapterLinkItems) {
                        chapterLinkItem.Id = 0;
                        chapterLinkItem.To = book.AllChapters.First(x => x.Id == chapterLinkItem.To.Id);
                        chapterLinkItem.From = book.AllChapters.First(x => x.Id == chapterLinkItem.From.Id);

                        //states.AddRange(chapterLinkItem.HeroStatesChanging ?? new List<StateChange>());
                        //states.AddRange(chapterLinkItem.RequirementStates ?? new List<StateValue>());
                        throw new NotImplementedException();
                    }

                    linkItems.AddRange(chapterLinkItems);
                    chapter.Book = book;
                }

                const char nbsp = (char)160;// code of nbsp
                const char sp = (char)32;// code of simple space

                foreach (var state in states) {
                    state.Id = 0;
                    state.StateType.Id = 0;
                    state.StateType.Owner = User;
                }

                states.ForEach(StateValueRepository.CheckAndSave);

                foreach (var @event in book.AllChapters) {
                    @event.LinksFromThisChapter = new List<ChapterLinkItem>();
                }

                BookRepository.Save(book);

                EventLinkItemRepository.Save(linkItems);
                EventLinkItemRepository.RemoveDuplicates();
            }

            return book.Id;
        }

        public FrontChapter ChangeRootEvent(long bookId, long eventId)
        {
            var book = BookRepository.Get(bookId);
            var @event = ChapterRepository.Get(eventId);
            book.RootChapter = @event;
            BookRepository.Save(book);

            var frontChapter = new FrontChapter(@event);

            return frontChapter;
        }

        [AcceptVerbs("GET")]
        public void BookCompleted(long bookId)
        {
            var book = BookRepository.Get(bookId);
            if (User.BooksAreReaded == null)
                User.BooksAreReaded = new List<UserWhoReadBook>();
            if (User.BooksAreReaded.All(x => x.Id != book.Id)) {
                User.BooksAreReaded.Add(new UserWhoReadBook { User = User, Book = book });
                throw new NotImplementedException();
                UserRepository.Save(User);
            }
        }


        public bool SaveEvaluation(FrontEvaluation frontEvaluation)
        {
            var evaluation = frontEvaluation.ToDbModel();
            var book = BookRepository.Get(evaluation.Book.Id);
            evaluation.Owner = User;
            evaluation.Book = book;
            evaluation.Created = DateTime.Now;

            EvaluationRepository.Save(evaluation);

            return true;
        }
    }
}
