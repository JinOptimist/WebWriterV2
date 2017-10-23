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
        private IBookRepository BookRepository { get; set; }
        private IGenreRepository GenreRepository { get; set; }
        private IUserRepository UserRepository { get; set; }
        private IChapterRepository EventRepository { get; set; }
        private IStateRepository StateRepository { get; set; }
        private IThingRepository ThingRepository { get; set; }
        private IChapterLinkItemRepository EventLinkItemRepository { get; set; }
        private IEvaluationRepository EvaluationRepository { get; set; }

        public ChapterController(IBookRepository bookRepository, IGenreRepository genreRepository, IUserRepository userRepository, 
            IChapterRepository eventRepository, IStateRepository stateRepository, IThingRepository thingRepository, 
            IChapterLinkItemRepository eventLinkItemRepository, IEvaluationRepository evaluationRepository)
        {
            BookRepository = bookRepository;
            GenreRepository = genreRepository;
            UserRepository = userRepository;
            EventRepository = eventRepository;
            StateRepository = stateRepository;
            ThingRepository = thingRepository;
            EventLinkItemRepository = eventLinkItemRepository;
            EvaluationRepository = evaluationRepository;
        }

        
    }
}
