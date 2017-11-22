using Dao.IRepository;
using Dao.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebWriterV2.Dao
{
    public class RepositoryContainer
    {
        public RepositoryContainer(IBookRepository bookRepository, IGenreRepository genreRepository, 
            IUserRepository userRepository, IChapterRepository eventRepository, IStateValueRepository stateRepository, 
            IThingRepository thingRepository, IChapterLinkItemRepository eventLinkItemRepository, 
            IEvaluationRepository evaluationRepository)
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

        public IBookRepository BookRepository { get; set; }
        public IGenreRepository GenreRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IChapterRepository EventRepository { get; set; }
        public IStateValueRepository StateRepository { get; set; }
        public IThingRepository ThingRepository { get; set; }
        public IChapterLinkItemRepository EventLinkItemRepository { get; set; }
        public IEvaluationRepository EvaluationRepository { get; set; }
    }
}