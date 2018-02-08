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
using WebWriterV2.RpgUtility;
using WebWriterV2.VkUtility;

namespace WebWriterV2.Controllers
{
    public class TravelController : BaseApiController
    {
        public TravelController(ITravelRepository travelRepository, ITravelStepRepository travelStepRepository, IBookRepository bookRepository, IChapterLinkItemRepository chapterLinkItemRepository)
        {
            TravelRepository = travelRepository;
            TravelStepRepository = travelStepRepository;
            BookRepository = bookRepository;
            ChapterLinkItemRepository = chapterLinkItemRepository;
        }

        private ITravelRepository TravelRepository { get; }
        private ITravelStepRepository TravelStepRepository { get; }
        private IBookRepository BookRepository { get; }
        private IChapterLinkItemRepository ChapterLinkItemRepository { get; }


        [AcceptVerbs("GET")]
        public FrontTravel Get(long id)
        {
            var travel = TravelRepository.Get(id);
            return new FrontTravel(travel);
        }

        [AcceptVerbs("GET")]
        public FrontTravel GetByBook(long bookId)
        {
            var travel = TravelRepository.GetByBookAndUser(bookId, User.Id);
            if (travel == null) {
                var book = BookRepository.Get(bookId);
                travel = new Travel {
                    Reader = User,
                    StartTime = DateTime.Now,
                    Book = book,
                    CurrentChapter = book.RootChapter
                };
                travel = TravelRepository.Save(travel);
            }
            return new FrontTravel(travel);
        }

        [AcceptVerbs("GET")]
        public FrontChapter Choice(long travelId, long linkItemId)
        {
            var link = ChapterLinkItemRepository.Get(linkItemId);
            var travel = TravelRepository.Get(travelId);
            var step = new TravelStep {
                DateTime = DateTime.Now,
                Travel = travel,
                Сhoice = link
            };
            travel.CurrentChapter = link.To;
            TravelStepRepository.Save(step);
            TravelRepository.Save(travel);
            return new FrontChapter(link.To);
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
    }
}
