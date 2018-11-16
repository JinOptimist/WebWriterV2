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
        public QuestionnaireController(IQuestionnaireRepository questionnaireRepository, IQuestionRepository questionRepository, IQuestionAnswerRepository questionAnswerRepository, IQuestionnaireResultRepository questionnaireResultRepository)
        {
            QuestionnaireRepository = questionnaireRepository;
            QuestionRepository = questionRepository;
            QuestionAnswerRepository = questionAnswerRepository;
            QuestionnaireResultRepository = questionnaireResultRepository;
        }

        private IQuestionnaireRepository QuestionnaireRepository { get; set; }
        private IQuestionRepository QuestionRepository { get; set; }
        private IQuestionAnswerRepository QuestionAnswerRepository { get; set; }
        private IQuestionnaireResultRepository QuestionnaireResultRepository { get; set; }

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

        [AcceptVerbs("POST")]
        public FrontQuestionAnswer SaveQuestionAnswer(FrontQuestionAnswer frontQuestionAnswer)
        {
            var questionAnswer = frontQuestionAnswer.ToDbModel();
            questionAnswer = QuestionAnswerRepository.Save(questionAnswer);
            return new FrontQuestionAnswer(questionAnswer);
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
                throw new UnauthorizedAccessException($"userId-{User.Id} try to remove Questionnaire");
            }
            QuestionnaireRepository.Remove(id);

            return true;
        }

        [AcceptVerbs("GET")]
        public bool RemoveQuestion(long id)
        {
            if (User.UserType != UserType.Admin) {
                throw new UnauthorizedAccessException($"userId-{User.Id} try to remove Question");
            }
            QuestionRepository.Remove(id);

            return true;
        }

        [AcceptVerbs("GET")]
        public bool RemoveQuestionAnswer(long id)
        {
            if (User.UserType != UserType.Admin) {
                throw new UnauthorizedAccessException($"userId-{User.Id} try to remove QuestionAnswer");
            }
            QuestionAnswerRepository.Remove(id);

            return true;
        }

        [AcceptVerbs("POST")]
        public bool SaveQuestionnaireResult(FrontQuestionnaireResult fontQuestionnaireResult)
        {
            var questionnaireResult = fontQuestionnaireResult.ToDbModel();
            QuestionnaireResultRepository.Save(questionnaireResult);
            return true;
        }
    }
}
