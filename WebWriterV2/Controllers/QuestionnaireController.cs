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
    public class QuestionnaireController : BaseApiController
    {
        public QuestionnaireController(IQuestionnaireRepository questionnaireRepository, IQuestionRepository questionRepository)
        {
            QuestionnaireRepository = questionnaireRepository;
            QuestionRepository = questionRepository;
        }

        private IQuestionnaireRepository QuestionnaireRepository { get; set; }
        private IQuestionRepository QuestionRepository { get; set; }
        

        [AcceptVerbs("POST")]
        public FrontQuestionnaire Save(FrontQuestionnaire FrontQuestionnaire)
        {
            var questionnaire = FrontQuestionnaire.ToDbModel();
            questionnaire = QuestionnaireRepository.Save(questionnaire);
            return new FrontQuestionnaire(questionnaire);
        }

        [AcceptVerbs("POST")]
        public FrontQuestion SaveQuestion(FrontQuestion frontQuestion)
        {
            var question = frontQuestion.ToDbModel();
            question = QuestionRepository.Save(question);
            return new FrontQuestion(question);
        }

        [AcceptVerbs("GET")]
        public FrontQuestionnaire Get(long id)
        {
            var article = QuestionnaireRepository.Get(id);
            var FrontQuestionnaire = new FrontQuestionnaire(article);
            return FrontQuestionnaire;
        }

        [AcceptVerbs("GET")]
        public List<FrontQuestionnaire> GetAll()
        {
            var articles = QuestionnaireRepository.GetAll();
            var FrontQuestionnaire = articles.Select(x => new FrontQuestionnaire(x)).ToList();
            return FrontQuestionnaire;
        }

        [AcceptVerbs("GET")]
        public bool Remove(long id)
        {
            if (User.UserType != UserType.Admin) {
                throw new UnauthorizedAccessException($"userId-{User.Id} try to remove article");
            }
            QuestionnaireRepository.Remove(id);

            return true;
        }
    }
}
