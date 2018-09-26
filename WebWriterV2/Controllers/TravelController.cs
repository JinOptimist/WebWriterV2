using Dao.IRepository;
using Dao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebWriterV2.FrontModels;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.Controllers
{
    public class TravelController : BaseApiController
    {
        public TravelController(ITravelRepository travelRepository, ITravelStepRepository travelStepRepository, IBookRepository bookRepository, IChapterLinkItemRepository chapterLinkItemRepository, IStateTypeRepository stateTypeRepository, IChapterRepository chapterRepository)
        {
            TravelRepository = travelRepository;
            TravelStepRepository = travelStepRepository;
            BookRepository = bookRepository;
            ChapterLinkItemRepository = chapterLinkItemRepository;
            StateTypeRepository = stateTypeRepository;
            ChapterRepository = chapterRepository;
        }

        private ITravelRepository TravelRepository { get; }
        private ITravelStepRepository TravelStepRepository { get; }
        private IBookRepository BookRepository { get; }
        private IChapterLinkItemRepository ChapterLinkItemRepository { get; }
        private IStateTypeRepository StateTypeRepository { get; }
        private IChapterRepository ChapterRepository { get; }


        [AcceptVerbs("GET")]
        public FrontTravel Get(long id, long stepId)
        {
            if (User == null) {
                throw new Exception("Only login user can user travel");
            }

            var step = TravelStepRepository.Get(stepId);
            var travel = TravelRepository.Get(id);
            return new FrontTravel(travel, step);
        }

        [AcceptVerbs("GET")]
        public FrontTravel GetByBook(long bookId)
        {
            Travel travel;
            var book = BookRepository.Get(bookId);
            if (User == null) {
                throw new Exception("Unxpected using of method. Only login user must use this method");
            }

            travel = TravelRepository.GetByBookAndUser(bookId, User.Id);
            if (travel == null) {
                travel = new Travel() {
                    Reader = User,
                    StartTime = DateTime.Now,
                    Book = book
                };
                travel = TravelRepository.Save(travel);

                var step = new TravelStep {
                    DateTime = DateTime.Now,
                    Travel = travel,
                    CurrentChapter = book.RootChapter
                };
                travel.CurrentStep = step;
                TravelRepository.Save(travel);
            }

            return new FrontTravel(travel);
        }

        [AcceptVerbs("GET")]
        public FrontTravel Choice(long travelId, long linkItemId)
        {
            var travel = TravelRepository.Get(travelId);
            var link = ChapterLinkItemRepository.Get(linkItemId);
            var fromCurrentStepWeCanDoStep = travel.CurrentStep.CurrentChapter.LinksFromThisChapter.Any(x => x.Id == linkItemId);
            if (!fromCurrentStepWeCanDoStep) {
                throw new Exception($"You try to change the past. User {User.Id}. Travel {travel.Id}. Step from passed chapted ch.Id {link.From.Id}");
            }

            var newStep = new TravelStep {
                DateTime = DateTime.Now,
                Travel = travel,
                Choice = link,
                CurrentChapter = link.To,
                PrevStep = travel.CurrentStep
            };
            TravelStepRepository.Save(newStep);
            travel.CurrentStep = newStep;

            StateHelper.ApplyChangeToTravel(travel, link.StateChanging);
            TravelRepository.Save(travel);
            return new FrontTravel(travel, newStep);
        }

        [AcceptVerbs("GET")]
        public List<FrontTravel> GetByUserId(long userId)
        {
            var travels = TravelRepository.GetByUser(userId);
            return travels.Select(x => new FrontTravel(x)).ToList();
        }

        [AcceptVerbs("GET")]
        public bool Remove(long id)
        {
            TravelRepository.Remove(id);
            return true;
        }

        [AcceptVerbs("GET")]
        public FrontTravelIsEnded TravelIsEnded(long id)
        {
            var travel = TravelRepository.Get(id);
            travel.IsTravelEnd = true;
            travel.FinishTime = DateTime.Now;
            TravelRepository.Save(travel);
            return new FrontTravelIsEnded(travel);
        }

        /// <summary>
        /// Temp method to remove all travels
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        public string RemoveAllTravel(string key)
        {
            if (User?.UserType != UserType.Admin || key != "32167") {
                return $"No";
            }

            var travels = TravelRepository.GetAll();
            var count = travels.Count;
            var stepCount = travels.SelectMany(x => x.Steps).Count();
            TravelRepository.Remove(travels);
            return $"We remove {count} travels and {stepCount} steps";
        }
    }
}
