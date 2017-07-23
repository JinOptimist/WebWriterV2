using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dao.IRepository;
using Dao.Model;
using Newtonsoft.Json;
using NLog;
using WebWriterV2.RpgUtility;
using WebWriterV2.VkUtility;
using Dao;
using Dao.Repository;
using WebWriterV2.FrontModels;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace WebWriterV2.Controllers
{
    public class RpgController : Controller
    {
        private int _priceOfRestore = 5;

        private const string AdminName = "admin";
        private const string AdminPassword = "32167";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly WriterContext _context = new WriterContext();

        #region Repository
        public IEventRepository EventRepository { get; }
        public IEventLinkItemRepository EventLinkItemRepository { get; }
        public IBookRepository BookRepository { get; set; }
        public IHeroRepository HeroRepository { get; set; }
        public IStateRepository StateRepository { get; set; }
        public IStateTypeRepository StateTypeRepository { get; set; }
        public IThingSampleRepository ThingSampleRepository { get; set; }
        public IThingRepository ThingRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IEvaluationRepository EvaluationRepository { get; set; }
        public IGenreRepository GenreRepository { get; set; }
        #endregion

        public RpgController()
        {
            EventRepository = new EventRepository(_context);
            EventLinkItemRepository = new EventLinkItemRepository(_context);
            BookRepository = new BookRepository(_context);
            HeroRepository = new HeroRepository(_context);
            StateRepository = new StateRepository(_context);
            StateTypeRepository = new StateTypeRepository(_context);
            ThingSampleRepository = new ThingSampleRepository(_context);
            ThingRepository = new ThingRepository(_context);
            UserRepository = new UserRepository(_context);
            EvaluationRepository = new EvaluationRepository(_context);
            GenreRepository = new GenreRepository(_context);

            //using (var scope = StaticContainer.Container.BeginLifetimeScope())
            //{
            //    EventRepository = scope.Resolve<IEventRepository>();
            //    BookRepository = scope.Resolve<IBookRepository>();
            //    HeroRepository = scope.Resolve<IHeroRepository>();
            //    SkillRepository = scope.Resolve<ISkillRepository>();
            //}
        }

        public ActionResult RouteForAngular(string url)
        {
            return View("Index");
        }

        public ActionResult Index()
        {
            return View();
        }

        /* ************** User ************** */
        public JsonResult Login(string username, string password)
        {
            var user = UserRepository.Login(username, password);
            FrontUser frontUser = null;
            if (user != null) {
                frontUser = new FrontUser(user);
            }
            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontUser),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult Register(string userJson)
        {
            var frontUser = SerializeHelper.Deserialize<FrontUser>(userJson);
            var user = frontUser.ToDbModel();
            user.ConfirmCode = RandomHelper.RandomString(RandomHelper.RandomInt(10, 20));
            user = UserRepository.Save(user);
            frontUser = new FrontUser(user);

            var relativeUrl = Url.Action("ConfirmRegister", new { userId = user.Id, confirmCode = user.ConfirmCode });
            var url = EmailHelper.ToAbsoluteUrl(relativeUrl);

            var body = $"Пожалуйста подтвердите регистрацию. Для этого достаточно перейти по ссылке {url}";
            var title = "Интерактивная книга. Регистрация";
            EmailHelper.Send(user.Email, title, body);

            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontUser),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult ConfirmRegister(int userId, string confirmCode)
        {
            var user = UserRepository.Get(userId);
            if (user.ConfirmCode == confirmCode) {
                user.ConfirmCode = null;
                UserRepository.Save(user);
                return RedirectToAction("RouteForAngular", new { url = "AngularRoute/registrationConfirm" });
            } else {
                return null;
            }
            
        }

        public JsonResult GetUserById(long userId)
        {
            var user = UserRepository.Get(userId);
            var frontUser = new FrontUser(user);
            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontUser),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddBookmark(long eventId, string heroJson)
        {
            var userId = CurrentUserId();
            var user = UserRepository.Get(userId);
            var currentEvent = EventRepository.Get(eventId);

            var fronHero = SerializeHelper.Deserialize<FrontHero>(heroJson);
            var hero = fronHero.ToDbModel();

            if (hero.Id > 0) {
                HeroRepository.Remove(hero.Id);
                hero.Id = 0;
            }
            HeroRepository.RemoveByBook(currentEvent.Book.Id, userId);

            hero.Owner = user;
            hero.CurrentEvent = currentEvent;
            hero.Name = hero.Name ?? "Just a Hero";

            HeroRepository.Save(hero);

            return new JsonResult {
                Data = JsonConvert.SerializeObject(true),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveUser(long userId)
        {
            //TODO check is user can remove another user
            UserRepository.Remove(userId);
            return new JsonResult {
                Data = JsonConvert.SerializeObject(true),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult BecomeWriter()
        {
            var userId = CurrentUserId();
            var user = UserRepository.Get(userId);
            if (user.UserType == UserType.Reader) {
                user.UserType = UserType.Writer;
                UserRepository.Save(user);
            } else {
                //TODO log error behavior
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /* ************** Utility for enum ************** */
        public JsonResult GetListRequirementType()
        {
            var listRequirementType = Enum.GetValues(typeof(RequirementType)).Cast<RequirementType>();
            var listNameValue = listRequirementType.Select(requirementType => new FrontEnum(requirementType));

            return new JsonResult {
                Data = JsonConvert.SerializeObject(listNameValue),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /* ************** Genre ************** */
        public JsonResult GetGenres()
        {
            var genres = GenreRepository.GetAll();
            var frontGenres = genres.Select(x => new FrontGenre(x)).ToList();

            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontGenres),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddGenre(string name, string desc)
        {
            var genre = new Genre {
                Name = name,
                Desc = desc
            };
            genre = GenreRepository.Save(genre);
            var frontGenre = new FrontGenre(genre);

            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontGenre),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SaveGenre(string jsonGenre)
        {
            var frontGenre = SerializeHelper.Deserialize<FrontGenre>(jsonGenre);
            var genre = frontGenre.ToDbModel();

            GenreRepository.Save(genre);

            frontGenre = new FrontGenre(genre);

            return new JsonResult {
                Data = SerializeHelper.Serialize(genre),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveGenre(int genreId)
        {
            GenreRepository.Remove(genreId);
            return new JsonResult {
                Data = JsonConvert.SerializeObject(true),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /* ************** Hero ************** */
        public JsonResult GetHero(long heroId)
        {
            var hero = HeroRepository.Get(heroId);
            if (hero == null) {
                var stateTypes = StateTypeRepository.GetAll();
                hero = GenerateData.GetDefaultHero(stateTypes);
            }

            var a = hero.Inventory?.FirstOrDefault();

            var frontHero = new FrontHero(hero);
            return new JsonResult {
                Data = SerializeHelper.Serialize(frontHero),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetHeroes()
        {
            var heroes = HeroRepository.GetAll();
            var frontHeroes = heroes.Select(x => new FrontHero(x)).ToList();
            return new JsonResult {
                Data = SerializeHelper.Serialize(frontHeroes),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SaveHero(string jsonHero)
        {
            var frontHero = SerializeHelper.Deserialize<FrontHero>(jsonHero);
            var hero = frontHero.ToDbModel();

            HeroRepository.Save(hero);
            frontHero.Id = hero.Id;

            return new JsonResult {
                Data = SerializeHelper.Serialize(frontHero),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveHero(long id)
        {
            var hero = HeroRepository.Get(id);
            var things = hero.Inventory.ToList();
            ThingRepository.Remove(things);

            HeroRepository.Remove(id);
            return new JsonResult {
                Data = true,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveAllHeroes()
        {
            var h = HeroRepository.GetAll();
            h.ForEach(HeroRepository.Remove);
            return new JsonResult {
                Data = true,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetEnemy()
        {
            var heroes = HeroRepository.GetAll();
            var frontHero = new FrontHero(heroes.LastOrDefault());
            return new JsonResult {
                Data = SerializeHelper.Serialize(frontHero),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetDefaultHero()
        {
            var stateTypes = StateTypeRepository.GetAll();

            var hero = GenerateData.GetDefaultHero(stateTypes);
            var frontHero = new FrontHero(hero);
            return new JsonResult {
                Data = SerializeHelper.Serialize(frontHero),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /* ************** Evaluation ************** */
        public JsonResult SaveEvaluation(string evaluationJson)
        {
            var frontEvaluation = SerializeHelper.Deserialize<FrontEvaluation>(evaluationJson);
            var evaluation = frontEvaluation.ToDbModel();
            var user = UserRepository.Get(CurrentUserId());
            var book = BookRepository.Get(evaluation.Book.Id);
            evaluation.Owner = user;
            evaluation.Book = book;
            evaluation.Created = DateTime.Now;

            EvaluationRepository.Save(evaluation);

            return new JsonResult {
                Data = SerializeHelper.Serialize(true),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /* ************** Thing ************** */
        public JsonResult GetThingSamples()
        {
            var thingSamples = ThingSampleRepository.GetAll();
            var thingSamplesFront = thingSamples.Select(x => new FrontThingSample(x)).ToList();

            return new JsonResult {
                Data = JsonConvert.SerializeObject(thingSamplesFront),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddThing(string name, string desc)
        {
            var user = UserRepository.Get(CurrentUserId());
            var thingSample = new ThingSample {
                Name = name,
                Desc = desc,
                Owner = user
            };

            var savedThingSample = ThingSampleRepository.Save(thingSample);
            var frontThingSample = new FrontThingSample(savedThingSample);
            return new JsonResult {
                Data = SerializeHelper.Serialize(frontThingSample),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveThing(long thingId)
        {
            //TODO check permission to remove thing
            ThingSampleRepository.Remove(thingId);
            return new JsonResult {
                Data = SerializeHelper.Serialize(true),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /* ************** State ************** */
        public JsonResult GetStateTypesAvailbleForUser()
        {
            var userId = CurrentUserId();
            var stateTypes = StateTypeRepository.AvailableForUse(userId);
            var frontStateTypes = stateTypes.Select(x => new FrontStateType(x)).ToList();

            return new JsonResult {
                Data = SerializeHelper.Serialize(frontStateTypes),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetStateTypesAvailbleForEdit()
        {
            var userId = CurrentUserId();
            var stateTypes = StateTypeRepository.AvailableForEdit(userId);
            var frontStateTypes = stateTypes.Select(x => new FrontStateType(x)).ToList();

            return new JsonResult {
                Data = SerializeHelper.Serialize(frontStateTypes),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult ChangeState(long stateId, long delta)
        {
            var state = StateRepository.Get(stateId);
            state.Number += delta;
            var fake = state.StateType;
            StateRepository.Save(state);

            var frontHero = new FrontState(state);
            return new JsonResult {
                Data = SerializeHelper.Serialize(frontHero),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddState(string name, string desc, bool hideFromReader)
        {
            var userId = CurrentUserId();
            User user = UserRepository.Get(userId);
            var stateType = new StateType {
                Name = name,
                Desc = desc,
                HideFromReader = hideFromReader,
                Owner = user,
            };
            var savedStateType = StateTypeRepository.Save(stateType);
            var frontStateType = new FrontStateType(savedStateType);
            return new JsonResult {
                Data = SerializeHelper.Serialize(frontStateType),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult EditStateType(string jsonStateType)
        {
            var frontStateType = SerializeHelper.Deserialize<FrontStateType>(jsonStateType);
            var stateType = frontStateType.ToDbModel();
            var userId = CurrentUserId();
            if (stateType.Owner.Id != userId) {
                throw new Exception("You can't edit state types which was created by another user");
            }
            stateType.Owner = UserRepository.Get(stateType.Owner.Id);
            StateTypeRepository.Save(stateType);
            return new JsonResult {
                Data = SerializeHelper.Serialize(true),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveState(long stateId)
        {
            StateTypeRepository.Remove(stateId);
            return new JsonResult {
                Data = SerializeHelper.Serialize(true),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /* ************** Book ************** */
        public JsonResult GetBook(long id)
        {
            var book = BookRepository.Get(id);
            var frontBook = new FrontBook(book, true);
            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontBook),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetBooks(long? userId)
        {
            List<Book> books;
            if (userId.HasValue) {
                books = BookRepository.GetByUser(userId.Value);
            } else {
                books = BookRepository.GetAll();
            }
            var frontBooks = books.Select(x => new FrontBook(x)).ToList();
            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontBooks),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveBook(long id)
        {
            BookRepository.Remove(id);

            return new JsonResult {
                Data = JsonConvert.SerializeObject(true),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SaveBook(string jsonBook)
        {
            var frontBook = SerializeHelper.Deserialize<FrontBook>(jsonBook);
            var book = frontBook.ToDbModel();
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

            frontBook = new FrontBook(book, true);

            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontBook),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult ImportBook(string jsonBook)
        {
            var frontBook = SerializeHelper.Deserialize<FrontBook>(jsonBook);
            var book = frontBook.ToDbModel();

            var bookName = BookRepository.GetByName(book.Name);
            if (bookName == null) {
                var currentUser = UserRepository.Get(CurrentUserId());
                book.Id = 0;
                book.Owner = currentUser;
                var things = new List<Thing>();
                var states = new List<State>();
                var linkItems = new List<EventLinkItem>();

                foreach (var @event in book.AllEvents) {
                    if (book.RootEvent.Id == @event.Id) {
                        @event.ForRootBook = book;
                    }

                    var eventLinkItems = @event.LinksFromThisEvent;
                    eventLinkItems.AddRange(@event.LinksToThisEvent);
                    foreach (var eventLinkItem in eventLinkItems) {
                        eventLinkItem.Id = 0;
                        eventLinkItem.To = book.AllEvents.First(x => x.Id == eventLinkItem.To.Id);
                        eventLinkItem.From = book.AllEvents.First(x => x.Id == eventLinkItem.From.Id);
                    }

                    linkItems.AddRange(eventLinkItems);
                    @event.Book = book;
                }

                foreach (var @event in book.AllEvents) {
                    @event.Id = 0;
                    things.AddRange(@event.RequirementThings ?? new List<Thing>());
                    things.AddRange(@event.ThingsChanges ?? new List<Thing>());
                    states.AddRange(@event.HeroStatesChanging ?? new List<State>());
                    states.AddRange(@event.RequirementStates ?? new List<State>());
                }

                /* Process Things connections */
                states.AddRange(things.SelectMany(x => x.ThingSample.PassiveStates ?? new List<State>()));
                states.AddRange(things.SelectMany(x => x.ThingSample.UsingEffectState ?? new List<State>()));

                /* Process Characteristics connections */
                foreach (var thing in things) {
                    thing.Id = 0;
                    thing.Hero = null;
                    thing.ThingSample.Id = 0;
                    thing.ThingSample.Owner = currentUser;
                }

                var nbsp = (char)160;// code of nbsp
                var sp = (char)32;// code of simple space

                foreach (var state in states) {
                    state.Id = 0;
                    state.StateType.Id = 0;
                    state.StateType.Owner = currentUser;
                }

                states.ForEach(StateRepository.CheckAndSave);
                things.ForEach(ThingRepository.CheckAndSave);

                foreach (var @event in book.AllEvents) {
                    @event.LinksFromThisEvent = new List<EventLinkItem>();
                }

                BookRepository.Save(book);

                EventLinkItemRepository.Save(linkItems);
                EventLinkItemRepository.RemoveDuplicates();
            }

            return new JsonResult {
                Data = book.Id,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult ChangeRootEvent(long bookId, long eventId)
        {
            var book = BookRepository.Get(bookId);
            var @event = EventRepository.Get(eventId);
            book.RootEvent = @event;
            BookRepository.Save(book);

            var frontEvent = new FrontEvent(@event);

            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontEvent),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult BookCompleted(long bookId)
        {
            var userId = CurrentUserId();
            var user = UserRepository.Get(userId);

            var book = BookRepository.Get(bookId);
            if (user.BooksAreReaded == null)
                user.BooksAreReaded = new List<Book>();
            if (user.BooksAreReaded.All(x => x.Id != book.Id)) {
                user.BooksAreReaded.Add(book);
                UserRepository.Save(user);
            }

            return new JsonResult {
                Data = true,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /* ************** Event ************** */
        public JsonResult GetEndingEvents(long bookId)
        {
            var events = EventRepository.GetEndingEvents(bookId);
            var frontEvents = events.Select(x => new FrontEvent(x)).ToList();
            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontEvents),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetNotAvailableEvents(long bookId)
        {
            var events = EventRepository.GetNotAvailableEvents(bookId);
            var frontEvents = events.Select(x => new FrontEvent(x)).ToList();
            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontEvents),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetEvents(long bookId)
        {
            var events = EventRepository.GetAllEventsByBook(bookId);
            var frontEvents = events.Select(x => new FrontEvent(x)).ToList();
            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontEvents),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetEvent(long id)
        {
            var eventFromDb = EventRepository.Get(id);
            var frontEvent = new FrontEvent(eventFromDb);
            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontEvent),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetEventForTravel(long eventId, long heroId)
        {
            var eventDb = EventRepository.Get(eventId);
            var hero = HeroRepository.Get(heroId);
            eventDb.LinksFromThisEvent.FilterLink(hero);
            var frontEvent = new FrontEvent(eventDb);
            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontEvent),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetEventForTravelWithHero(long eventId, string heroJson, bool applyChanges)
        {
            var eventDb = EventRepository.Get(eventId);
            var frontHero = JsonConvert.DeserializeObject<FrontHero>(heroJson);
            var hero = frontHero.ToDbModel();
            hero.CurrentEvent = eventDb;

            if (applyChanges) {
                eventDb.EventChangesApply(hero);
            }

            eventDb.LinksFromThisEvent.FilterLink(hero);

            var frontEvent = new FrontEvent(eventDb);
            frontHero = new FrontHero(hero);
            return new JsonResult {
                Data = JsonConvert.SerializeObject(new { frontEvent, frontHero }),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult EventChangesApplyToHero(long eventId, long heroId)
        {
            var eventDb = EventRepository.Get(eventId);
            var hero = HeroRepository.Get(heroId);

            eventDb.EventChangesApply(hero);

            HeroRepository.Save(hero);
            var frontHero = new FrontHero(hero);
            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontHero),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SaveEvent(string jsonEvent, long bookId)
        {
            var frontEvent = SerializeHelper.Deserialize<FrontEvent>(jsonEvent);
            var eventModel = frontEvent.ToDbModel();
            if (eventModel.Id == 0) {
                eventModel.Book = BookRepository.Get(bookId);
            }

            var eventFromDb = EventRepository.Save(eventModel);
            var frontEvents = new FrontEvent(eventFromDb);
            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontEvents),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SaveEventLink(string jsonEventLink)
        {
            var frontEvent = SerializeHelper.Deserialize<FrontEventLinkItem>(jsonEventLink);
            var eventLinkItem = frontEvent.ToDbModel();

            var linkItemFromDb = EventLinkItemRepository.Save(eventLinkItem);
            var frontEvents = new FrontEventLinkItem(linkItemFromDb);
            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontEvents),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveEventLink(long eventLinkId)
        {
            EventLinkItemRepository.Remove(eventLinkId);
            return new JsonResult {
                Data = JsonConvert.SerializeObject(true),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveEvent(long eventId)
        {
            EventRepository.Remove(eventId);

            return new JsonResult {
                Data = true,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddStateToEvent(long eventId, long stateTypeId, int stateValue)
        {
            var eventFromDb = EventRepository.Get(eventId);
            var stateType = StateTypeRepository.Get(stateTypeId);

            var state =
                eventFromDb.HeroStatesChanging.FirstOrDefault(
                    x => x.StateType.Id == stateType.Id)
                ?? new State {
                    StateType = stateType
                };

            state.Number = stateValue;

            // if new
            if (state.Id < 1)
                eventFromDb.HeroStatesChanging.Add(state);
            StateRepository.Save(state);
            EventRepository.Save(eventFromDb);
            var frontState = new FrontState(state);

            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontState),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveStateFromEvent(long stateId)
        {
            StateRepository.Remove(stateId);

            return new JsonResult {
                Data = JsonConvert.SerializeObject(true),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddReqStateToEvent(long eventId, long stateTypeId, int reqType, int stateValue)
        {
            var eventFromDb = EventRepository.Get(eventId);
            var stateType = StateTypeRepository.Get(stateTypeId);
            var requirementType = (RequirementType)reqType;

            var state =
                eventFromDb.RequirementStates.FirstOrDefault(
                    x => x.StateType.Id == stateType.Id)
                ?? new State {
                    StateType = stateType
                };

            state.Number = stateValue;
            state.RequirementType = requirementType;

            // if new
            if (state.Id < 1)
                eventFromDb.RequirementStates.Add(state);
            StateRepository.Save(state);
            EventRepository.Save(eventFromDb);
            var frontState = new FrontState(state);

            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontState),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveReqStateFromEvent(long stateId)
        {
            StateRepository.Remove(stateId);

            return new JsonResult {
                Data = JsonConvert.SerializeObject(true),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddRequirementThingToEvent(long eventId, long thingSampleId, int count)
        {
            var eventFromDb = EventRepository.Get(eventId);
            var thingSample = ThingSampleRepository.Get(thingSampleId);
            var thing = new Thing {
                Count = count,
                Hero = null,
                ThingSample = thingSample,
                ItemInUse = false
            };

            ThingRepository.Save(thing);
            eventFromDb.RequirementThings.Add(thing);
            EventRepository.Save(eventFromDb);
            var frontThing = new FrontThing(thing);

            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontThing),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveRequirementThingFromEvent(long eventId, long thingId)
        {
            var eventFromDb = EventRepository.Get(eventId);
            var thing = ThingRepository.Get(thingId);
            eventFromDb.RequirementThings.Remove(thing);
            EventRepository.Save(eventFromDb);
            ThingRepository.Remove(thing);

            return new JsonResult {
                Data = JsonConvert.SerializeObject(true),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult AddThingChangesToEvent(long eventId, long thingSampleId, int count)
        {
            var eventFromDb = EventRepository.Get(eventId);
            var thingSample = ThingSampleRepository.Get(thingSampleId);
            var thing = new Thing {
                Count = count,
                Hero = null,
                ThingSample = thingSample,
                ItemInUse = false
            };

            ThingRepository.Save(thing);
            eventFromDb.ThingsChanges.Add(thing);
            EventRepository.Save(eventFromDb);
            var frontThing = new FrontThing(thing);

            return new JsonResult {
                Data = JsonConvert.SerializeObject(frontThing),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult RemoveThingChangesFromEvent(long eventId, long thingId)
        {
            var eventFromDb = EventRepository.Get(eventId);
            var thing = ThingRepository.Get(thingId);
            eventFromDb.ThingsChanges.Remove(thing);
            EventRepository.Save(eventFromDb);
            ThingRepository.Remove(thing);

            return new JsonResult {
                Data = JsonConvert.SerializeObject(true),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult CreateNextChapter(long eventId)
        {
            var parentEvent = EventRepository.Get(eventId);
            var childEvent = new Event() {
                Name = parentEvent.Name + " продолжение",
                Desc = "продолжение",
                Book = parentEvent.Book,
            };
            childEvent = EventRepository.Save(childEvent);
            var eventLinkItem = new EventLinkItem() {
                From = parentEvent,
                To = childEvent,
                Text = "дальше"
            };
            EventLinkItemRepository.Save(eventLinkItem);

            var frontEvent = new FrontEvent(childEvent);

            return Json(frontEvent, JsonRequestBehavior.AllowGet);

        }

        /* ************** Init Db ************** */
        public JsonResult AddDefaultState()
        {
            var stateTypes = StateTypeRepository.GetAll();
            var stateTypeForAdding = new List<StateType>();
            var lifeStateType = new StateType {
                Name = "Жизни",
                Desc = "Закончаться жизни и вашему путешествию придёт конец"
            };
            var timeStateType = new StateType {
                Name = "Время",
                Desc = "Время всегда на исходе"
            };
            var hpStateType = new StateType {
                Name = "Здоровье",
                Desc = "Закончиться здоровье и ты труп"
            };

            if (stateTypes.All(x => x.Name != lifeStateType.Name)) {
                stateTypeForAdding.Add(lifeStateType);
            }
            if (stateTypes.All(x => x.Name != timeStateType.Name)) {
                stateTypeForAdding.Add(timeStateType);
            }
            if (stateTypes.All(x => x.Name != hpStateType.Name)) {
                stateTypeForAdding.Add(hpStateType);
            }

            if (stateTypeForAdding.Any()) {
                StateTypeRepository.Save(stateTypeForAdding);
            }

            return Json(stateTypeForAdding.Any(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddDefaultThing()
        {
            var thingSamples = ThingSampleRepository.GetAll();
            var thingSampleForAdding = new List<ThingSample>();
            var goldStateType = new ThingSample {
                Name = "Золото",
                Desc = "Прятный звон монет придаст вам толику хорошего настроения",
                IsUsed = false,
            };
            var creditStateType = new ThingSample {
                Name = "Кредиты",
                Desc = "Ваш счёт в межгалактическом банке сектора",
                IsUsed = false,
            };
            var keyStateType = new ThingSample {
                Name = "Ключ",
                Desc = "Знать бы где он понадобиться",
                IsUsed = false,
            };

            if (thingSamples.All(x => x.Name != goldStateType.Name)) {
                thingSampleForAdding.Add(goldStateType);
            }
            if (thingSamples.All(x => x.Name != creditStateType.Name)) {
                thingSampleForAdding.Add(creditStateType);
            }
            if (thingSamples.All(x => x.Name != keyStateType.Name)) {
                thingSampleForAdding.Add(keyStateType);
            }

            if (thingSampleForAdding.Any()) {
                ThingSampleRepository.Save(thingSampleForAdding);
            }

            return Json(thingSampleForAdding.Any(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Init()
        {
            /* Создаём StateType */
            var stateTypes = StateTypeRepository.GetAll();
            if (!stateTypes.Any()) {
                stateTypes = GenerateData.GenerateStateTypes();
                StateTypeRepository.Save(stateTypes);
            }

            /* Создаём ThingSamples */
            var thingSamples = ThingSampleRepository.GetAll();
            if (!thingSamples.Any()) {
                thingSamples = GenerateData.GenerateThingSample(stateTypes);
                ThingSampleRepository.Save(thingSamples);
            }

            /* Создаём Квесты. Чистый без евентов */
            var books = BookRepository.GetAll();
            if (!books.Any()) {
                var book1 = GenerateData.BookRat();
                BookRepository.Save(book1);

                var book2 = GenerateData.BookTower(stateTypes, thingSamples);
                BookRepository.Save(book2);
            }

            // Создаём Евенты с текстом но без связей
            //var events = EventRepository.GetAll();
            //if (!events.Any())
            //{
            //    events = GenerateData.GenerateEventsForBook(quest);
            //    foreach (var eve in events)
            //    {
            //        EventRepository.Save(eve);
            //    }
            //}

            // Создаём связи между Евентами
            //var eventLinkItemsDb = EventLinkItemRepository.GetAll();
            //if (!eventLinkItemsDb.Any())
            //{
            //    foreach (var currentEvent in events)
            //    {
            //        var eventLinkItems = GenerateData.CreateConnectionForEvents(events, currentEvent);
            //        if (eventLinkItems != null)
            //            EventLinkItemRepository.Save(eventLinkItems);
            //    }
            //}

            var answer = new {
                books = books.Any() ? "Уже существует" : "Добавили",
                //eventLinkItemsDb = eventLinkItemsDb.Any() ? "Уже существует" : "Добавили",
                thingSamples = thingSamples.Any() ? "Уже существует" : "Добавили",
            };

            return new JsonResult {
                Data = answer,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult ReInit()
        {
            var user = UserRepository.Get(CurrentUserId());
            if (user.UserType != UserType.Admin) {
                throw new Exception("You haven't permission to rebuild whole db");
            }

            var states = StateRepository.GetAll();
            StateRepository.Remove(states);

            var things = ThingRepository.GetAll();
            ThingRepository.Remove(things);

            var thingSamples = ThingSampleRepository.GetAll();
            ThingSampleRepository.Remove(thingSamples);

            var heroes = HeroRepository.GetAll();
            HeroRepository.Remove(heroes);

            var links = EventLinkItemRepository.GetAll();
            EventLinkItemRepository.Remove(links);

            var events = EventRepository.GetAll();
            EventRepository.Remove(events);

            var books = BookRepository.GetAll();
            BookRepository.Remove(books);

            var stateTypes = StateTypeRepository.GetAll();
            StateTypeRepository.Remove(stateTypes);

            return Init();
        }

        public JsonResult AddAdminUser()
        {
            var user = UserRepository.GetByName(AdminName);
            if (user == null) {
                user = new User {
                    Name = AdminName,
                    Password = AdminPassword,
                    UserType = UserType.Admin
                };
                user = UserRepository.Save(user);
            }
            if (user.UserType != UserType.Admin) {
                user.UserType = UserType.Admin;
                UserRepository.Save(user);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllUsers()
        {
            var user = UserRepository.Get(CurrentUserId());
            if (user == null || user.UserType != UserType.Admin) {
                return Json("GoFuckYourSelf", JsonRequestBehavior.AllowGet);
            }

            var frontUsers = UserRepository.GetAll().Select(x=> new FrontUser(x));
            return Json(frontUsers, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllBooks()
        {
            var user = UserRepository.Get(CurrentUserId());
            if (user == null || user.UserType != UserType.Admin) {
                return Json("GoFuckYourSelf", JsonRequestBehavior.AllowGet);
            }

            var frontBooks = BookRepository.GetAll().Select(x => new FrontBook(x));
            return Json(frontBooks, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }

        private long CurrentUserId()
        {
            return long.Parse(Request.Cookies["userId"]?.Value ?? "-1");
        }
    }
}
