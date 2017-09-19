using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dao.IRepository;
using Dao.Model;
using Newtonsoft.Json;
using WebWriterV2.RpgUtility;
using WebWriterV2.VkUtility;
using Dao;
using Dao.Repository;
using WebWriterV2.FrontModels;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.IO;
using System.Text;
using WebWriterV2.RpgUtility.Dto;
using WebWriterV2.DI;
using Autofac;

namespace WebWriterV2.Controllers
{
    public class RpgController : MyController
    {
        private const string AdminName = "admin";
        private const string AdminPassword = "32167";

        //private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
            var container = StaticContainer.Container;

            EventRepository = container.Resolve<IEventRepository>();
            EventLinkItemRepository = container.Resolve<IEventLinkItemRepository>();
            BookRepository = container.Resolve<IBookRepository>();
            HeroRepository = container.Resolve<IHeroRepository>();
            StateRepository = container.Resolve<IStateRepository>();
            StateTypeRepository = container.Resolve<IStateTypeRepository>();
            ThingSampleRepository = container.Resolve<IThingSampleRepository>();
            ThingRepository = container.Resolve<IThingRepository>();
            UserRepository = container.Resolve<IUserRepository>();
            EvaluationRepository = container.Resolve<IEvaluationRepository>();
            GenreRepository = container.Resolve<IGenreRepository>();

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

        public ActionResult RegisterVkComplete(string code)
        {
            var redirectUri = "http://localhost:52079/rpg/RegisterVkComplete";
            var secret = "S8CbQO9K10jPNTuGKDUv";
            var urlAccessToken = "https://oauth.vk.com/access_token?"
                + $"client_id={4279045}&client_secret={secret}&redirect_uri={redirectUri}&code={code}";
            
            var request = (HttpWebRequest)WebRequest.Create(urlAccessToken);
            request.Method = "GET";

            var answer = "";
            using (var response = (HttpWebResponse)request.GetResponse()) {
                var reader = new StreamReader(response.GetResponseStream());
                var output = new StringBuilder();
                output.Append(reader.ReadToEnd());
                response.Close();
                answer = output.ToString();
            }

            var jsonReader = new JsonResponseReader<VkAccess>();
            var vkAc = jsonReader.ReadResponse(answer);

            return RedirectToAction("Index");
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
            var userId = User.Id;
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
            if (User.UserType == UserType.Reader) {
                User.UserType = UserType.Writer;
                UserRepository.Save(User);
            } else {
                //TODO log error behavior
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UploadAvatar(string data)
        {
            //example of base64 string 
            //data:image/png;base64,iVBORw0KGg...
            var dataIndex = data.IndexOf("base64", StringComparison.Ordinal) + 7;
            var mark = "data:image/";
            var extensionStart = data.IndexOf(mark) + mark.Length;
            var extensionEnd = data.IndexOf(";");
            var extension = data.Substring(extensionStart, extensionEnd - extensionStart);

            var clearData = data.Substring(dataIndex);
            var fileData = Convert.FromBase64String(clearData);
            var bytes = fileData.ToArray();

            var path = PathHelper.PathToAvatar(User.Id, extension);
            if (System.IO.File.Exists(path)) {
                System.IO.File.Delete(path);
            }
            
            using (var fileStream = System.IO.File.Create(path)) {
                fileStream.Write(bytes, 0, bytes.Length);
                //TODO investigate why we reload page on client after on server I close strea
                fileStream.Close();
            }


            User.AvatarUrl = PathHelper.PathToUrl(path);
            UserRepository.Save(User);

            return Json(JsonConvert.SerializeObject(User.AvatarUrl), JsonRequestBehavior.AllowGet);
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
            var thingSample = new ThingSample {
                Name = name,
                Desc = desc,
                Owner = User
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
            var stateTypes = StateTypeRepository.AvailableForUse(User.Id);
            var frontStateTypes = stateTypes.Select(x => new FrontStateType(x)).ToList();

            return new JsonResult {
                Data = SerializeHelper.Serialize(frontStateTypes),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetStateTypesAvailbleForEdit()
        {
            var stateTypes = StateTypeRepository.AvailableForEdit(User.Id);
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
            var stateType = new StateType {
                Name = name,
                Desc = desc,
                HideFromReader = hideFromReader,
                Owner = User,
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
            if (stateType.Owner.Id != User.Id) {
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
            var frontEvent = eventFromDb == null
                ? null
                : new FrontEvent(eventFromDb);
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

        public JsonResult SaveEvent(FrontEvent frontEvent, long bookId)
        {
            var eventModel = frontEvent.ToDbModel();
            var book = BookRepository.Get(bookId);
            eventModel.Book = book;

            long oldTextLength = 0;
            long oldNumberOfWords = 0;
            if (eventModel.Id != 0) {
                var oldEvent = EventRepository.Get(eventModel.Id);
                oldTextLength = oldEvent.Desc.Length;
                oldNumberOfWords = oldEvent.NumberOfWords;
            }

            eventModel.NumberOfWords = WordHelper.GetWordCount(eventModel.Desc);
            var eventFromDb = EventRepository.Save(eventModel);

            book.NumberOfChapters = book.NumberOfChapters - oldTextLength + eventModel.Desc.Length;
            book.NumberOfWords = book.NumberOfWords - oldNumberOfWords + eventModel.NumberOfWords;
            BookRepository.Save(book);

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
            var eventToRemove = EventRepository.Get(eventId);
            var book = BookRepository.Get(eventToRemove.Book.Id);
            book.NumberOfWords -= eventToRemove.NumberOfWords;
            book.NumberOfChapters -= eventToRemove.Desc.Length;
            EventRepository.Remove(eventId);
            BookRepository.Save(book);

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
                NumberOfWords = 1
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
            var books = BookRepository.GetAll(false);
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
            if (User.UserType != UserType.Admin) {
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

            var books = BookRepository.GetAll(false);
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
            if (User == null || User.UserType != UserType.Admin) {
                return Json("GoHuckYourSelf", JsonRequestBehavior.AllowGet);
            }

            var frontUsers = UserRepository.GetAll().Select(x=> new FrontUser(x));
            return Json(frontUsers, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllBooks()
        {
            if (!IsCurrentUserAdmin) {
                return Json("GoFuckYourSelf", JsonRequestBehavior.AllowGet);
            }

            var frontBooks = BookRepository.GetAll(false).Select(x => new FrontBook(x));
            return Json(frontBooks, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RecalculateBookSize()
        {
            if (!IsCurrentUserAdmin) {
                return Json("GoFuckYourSelf", JsonRequestBehavior.AllowGet);
            }

            var books = BookRepository.GetAll(false);
            foreach(var book in books) {
                book.NumberOfChapters = book.AllEvents.Sum(x => x.Desc.Length);
                book.NumberOfWords = book.AllEvents.Sum(x=>WordHelper.GetWordCount(x.Desc));
            }

            BookRepository.Save(books);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private bool IsCurrentUserAdmin {
            get
            {
                return User != null && User.UserType == UserType.Admin;
            }
        }
    }
}
