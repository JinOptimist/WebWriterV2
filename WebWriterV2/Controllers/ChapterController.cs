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
using WebWriterV2.VkUtility;

namespace WebWriterV2.Controllers
{
    public class ChapterController : BaseApiController
    {
        private IBookRepository BookRepository { get; set; }
        private IGenreRepository GenreRepository { get; set; }
        private IUserRepository UserRepository { get; set; }
        private IChapterRepository EventRepository { get; set; }
        private IStateValueRepository StateRepository { get; set; }
        private IChapterLinkItemRepository EventLinkItemRepository { get; set; }
        private IEvaluationRepository EvaluationRepository { get; set; }

        public ChapterController(IBookRepository bookRepository, IGenreRepository genreRepository, IUserRepository userRepository, 
            IChapterRepository eventRepository, IStateValueRepository stateRepository, 
            IChapterLinkItemRepository eventLinkItemRepository, IEvaluationRepository evaluationRepository)
        {
            BookRepository = bookRepository;
            GenreRepository = genreRepository;
            UserRepository = userRepository;
            EventRepository = eventRepository;
            StateRepository = stateRepository;
            EventLinkItemRepository = eventLinkItemRepository;
            EvaluationRepository = evaluationRepository;
        }

        
    }
}
