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
    public class BookController : BaseApiController
    {
        private IBookRepository BookRepository { get; }
        private IEvaluationRepository EvaluationRepository { get; }
        private IChapterLinkItemRepository EventLinkItemRepository { get; }
        private IChapterRepository EventRepository { get; }
        private IStateValueRepository StateRepository { get; }
        private IGenreRepository GenreRepository { get; }
        private IUserRepository UserRepository { get; }
        private IThingRepository ThingRepository { get; }
        


        // old GetBook
        public FrontBook Get(long id)
        {
            var book = BookRepository.Get(id);
            var frontBook = new FrontBook(book, true);
            return frontBook;
        }

        // old GetBooks(long? userId) with null
        public List<FrontBook> GetAll()
        {
            var books = BookRepository.GetAll(User == null || User.UserType != UserType.Admin);
            var frontBooks = books.Select(x => new FrontBook(x)).ToList();
            return frontBooks;
        }

        // old GetBooks(long? userId) with userId
        public List<FrontBook> GetByUser(long id)
        {
            var books = BookRepository.GetByUser(id);
            var frontBooks = books.Select(x => new FrontBook(x)).ToList();
            return frontBooks;
        }

        public void Remove(long id)
        {
            BookRepository.Remove(id);
        }

        //TODO
        public FrontBook SaveBook(FrontBook jsonBook)
        {
            //var frontBook = SerializeHelper.Deserialize<FrontBook>(jsonBook);
            var book = jsonBook.ToDbModel();
            var newGenre = book.Genre;
            var owner = UserRepository.Get(book.Owner.Id);
            book.Owner = owner;
            book = BookRepository.Save(book);

            if (newGenre != null) {
                var genre = GenreRepository.Get(newGenre.Id);
                if (genre.Books == null) {
                    genre.Books = new List<Book>();
                }
                genre.Books.Add(book);
                GenreRepository.Save(genre);
            }

            var frontBook = new FrontBook(book, true);
            return frontBook;
        }

        public long ImportBook(string jsonBook)
        {
            var frontBook = SerializeHelper.Deserialize<FrontBook>(jsonBook);
            var book = frontBook.ToDbModel();

            var bookName = BookRepository.GetByName(book.Name);
            if (bookName == null) {
                book.Id = 0;
                book.Owner = User;
                var things = new List<Thing>();
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

                        things.AddRange(chapterLinkItem.RequirementThings ?? new List<Thing>());
                        things.AddRange(chapterLinkItem.ThingsChanges ?? new List<Thing>());
                        states.AddRange(chapterLinkItem.HeroStatesChanging ?? new List<StateValue>());
                        states.AddRange(chapterLinkItem.RequirementStates ?? new List<StateValue>());
                    }

                    linkItems.AddRange(chapterLinkItems);
                    chapter.Book = book;
                }

                /* Process Things connections */
                states.AddRange(things.SelectMany(x => x.ThingSample.PassiveStates ?? new List<StateValue>()));
                states.AddRange(things.SelectMany(x => x.ThingSample.UsingEffectState ?? new List<StateValue>()));

                /* Process Characteristics connections */
                foreach (var thing in things) {
                    thing.Id = 0;
                    thing.Hero = null;
                    thing.ThingSample.Id = 0;
                    thing.ThingSample.Owner = User;
                }

                const char nbsp = (char)160;// code of nbsp
                const char sp = (char)32;// code of simple space

                foreach (var state in states) {
                    state.Id = 0;
                    state.StateType.Id = 0;
                    state.StateType.Owner = User;
                }

                states.ForEach(StateRepository.CheckAndSave);
                things.ForEach(ThingRepository.CheckAndSave);

                foreach (var @event in book.AllChapters) {
                    @event.LinksFromThisChapter = new List<ChapterLinkItem>();
                }

                BookRepository.Save(book);

                EventLinkItemRepository.Save(linkItems);
                EventLinkItemRepository.RemoveDuplicates();
            }

            return book.Id;
        }

        public FrontEvent ChangeRootEvent(long bookId, long eventId)
        {
            var book = BookRepository.Get(bookId);
            var @event = EventRepository.Get(eventId);
            book.RootChapter = @event;
            BookRepository.Save(book);

            var frontEvent = new FrontEvent(@event);

            return frontEvent;
        }

        [AcceptVerbs("GET")]
        public void BookCompleted(long bookId)
        {
            var book = BookRepository.Get(bookId);
            if (User.BooksAreReaded == null)
                User.BooksAreReaded = new List<Book>();
            if (User.BooksAreReaded.All(x => x.Id != book.Id)) {
                User.BooksAreReaded.Add(book);
                UserRepository.Save(User);
            }
        }

        [AcceptVerbs("GET", "POST")]
        public void PublishBook(long bookId, bool newValue)
        {
            var book = BookRepository.Get(bookId);
            book.IsPublished = newValue;
            BookRepository.Save(book);
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
