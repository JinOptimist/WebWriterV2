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
        public TravelController(ITravelRepository travelRepository, ITravelStepRepository travelStepRepository, IBookRepository bookRepository, IChapterLinkItemRepository chapterLinkItemRepository, IStateTypeRepository stateTypeRepository)
        {
            TravelRepository = travelRepository;
            TravelStepRepository = travelStepRepository;
            BookRepository = bookRepository;
            ChapterLinkItemRepository = chapterLinkItemRepository;
            StateTypeRepository = stateTypeRepository;
        }

        private ITravelRepository TravelRepository { get; }
        private ITravelStepRepository TravelStepRepository { get; }
        private IBookRepository BookRepository { get; }
        private IChapterLinkItemRepository ChapterLinkItemRepository { get; }
        private IStateTypeRepository StateTypeRepository { get; }

        [AcceptVerbs("GET")]
        public FrontTravel Get(long id)
        {
            var travel = TravelRepository.Get(id);
            return new FrontTravel(travel);
        }

        [AcceptVerbs("GET")]
        public FrontTravel GetByBook(long bookId)
        {
            Travel travel;
            var book = BookRepository.Get(bookId);
            if (User == null) {
                travel = new Travel() {
                    CurrentChapter = book.RootChapter,
                    Id = -1
                };
            } else {
                
                travel = TravelRepository.GetByBookAndUser(bookId, User.Id);
                if (travel == null) {

                    travel = new Travel {
                        Reader = User,
                        StartTime = DateTime.Now,
                        Book = book,
                        CurrentChapter = book.RootChapter
                    };
                    travel = TravelRepository.Save(travel);
                }
            }

            return new FrontTravel(travel);
        }

        [AcceptVerbs("GET")]
        public FrontChapter Choice(long travelId, long linkItemId)
        {
            var link = ChapterLinkItemRepository.Get(linkItemId);
            var travel = new Travel();
            if (travelId > 0) {
                travel = TravelRepository.Get(travelId);
                var step = new TravelStep {
                    DateTime = DateTime.Now,
                    Travel = travel,
                    Сhoice = link
                };
                travel.CurrentChapter = link.To;
                TravelStepRepository.Save(step);

                var changes = link.StateChanging.SingleOrDefault();
                if (changes != null) {

                    if (changes.Text == "УДАЛИТЬ") {
                        travel.State.Clear();
                    } else {
                        var defaultStateType = StateTypeRepository.GetDefault();
                        travel.State.Add(new StateValue() {
                            Travel = travel,
                            Text = changes.Text,
                            StateType = defaultStateType
                        });
                    }
                }

                TravelRepository.Save(travel);
            }
            
            return new FrontChapter(link.To, travel);
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
        public bool TravelIsEnd(long id)
        {
            var travel = TravelRepository.Get(id);
            travel.IsTravelEnd = true;
            travel.FinishTime = DateTime.Now;
            TravelRepository.Save(travel);
            return true;
        }
    }
}
