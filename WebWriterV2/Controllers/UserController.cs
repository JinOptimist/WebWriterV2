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
    public class UserController : BaseApiController
    {
        public UserController(IBookRepository bookRepository, IEvaluationRepository evaluationRepository, IChapterLinkItemRepository eventLinkItemRepository, IChapterRepository eventRepository, IStateValueRepository stateRepository, IGenreRepository genreRepository, IUserRepository userRepository)
        {
            BookRepository = bookRepository;
            EvaluationRepository = evaluationRepository;
            EventLinkItemRepository = eventLinkItemRepository;
            EventRepository = eventRepository;
            StateRepository = stateRepository;
            GenreRepository = genreRepository;
            UserRepository = userRepository;
        }

        private IBookRepository BookRepository { get; }
        private IEvaluationRepository EvaluationRepository { get; }
        private IChapterLinkItemRepository EventLinkItemRepository { get; }
        private IChapterRepository EventRepository { get; }
        private IStateValueRepository StateRepository { get; }
        private IGenreRepository GenreRepository { get; }
        private IUserRepository UserRepository { get; }

        [AcceptVerbs("GET")]
        public FrontUser Login(string username, string password)
        {
            var user = UserRepository.Login(username, password);
            FrontUser frontUser = null;
            if (user != null) {
                frontUser = new FrontUser(user);
            }

            return frontUser;
        }

    }
}
