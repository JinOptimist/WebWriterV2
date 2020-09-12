using Dal;
using Dal.IRepository;
using Dal.Model;
using Dal.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using WebWriterV2.DI;
using WebWriterV2.FrontModel.Email;
using WebWriterV2.FrontModels;
using WebWriterV2.RpgUtility;
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

            var questionAnswers = QuestionAnswerRepository.GetByQuestionVisibleIf(question);
            questionAnswers.ForEach(x => x.AffectVisibilityOfQuestions.RemoveAll(q => q.Id == question.Id));
            QuestionAnswerRepository.Save(questionAnswers);
            // dirty hack. I can't save question and add relation between existing item by one step
            long? visibleIf = frontQuestion.VisibleIf.FirstOrDefault();
            if (visibleIf.HasValue && visibleIf > 0) {
                var questionAnswer = QuestionAnswerRepository.Get(visibleIf.Value);
                questionAnswer.AffectVisibilityOfQuestions.Add(question);
                QuestionAnswerRepository.Save(questionAnswer);
            }

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
            var frontQuestionnaire = new FrontQuestionnaire(article);
            return frontQuestionnaire;
        }

        [AcceptVerbs("GET")]
        public List<FrontQuestionnaire> GetAll()
        {
            var questionnaires = QuestionnaireRepository.GetAll();
            var frontQuestionnaires = questionnaires.Select(x => new FrontQuestionnaire(x)).ToList();
            return frontQuestionnaires;
        }

        [AcceptVerbs("GET")]
        public List<FrontQuestionnaire> GetForWriter(long userId)
        {
            var questionnaires = QuestionnaireRepository.GetForWriter(userId);
            var frontQuestionnaires = questionnaires.Select(x => new FrontQuestionnaire(x)).ToList();
            return frontQuestionnaires;
        }

        [AcceptVerbs("GET")]
        public List<FrontQuestionnaireResult> GetAllQuestionnaireResults()
        {
            var questionnaireResults = QuestionnaireResultRepository.GetAll();
            var frontQuestionnaireResults = questionnaireResults.Select(x => new FrontQuestionnaireResult(x)).ToList();
            return frontQuestionnaireResults;
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
            questionnaireResult = QuestionnaireResultRepository.Save(questionnaireResult);


            var questionnaire = QuestionnaireRepository.Get(questionnaireResult.Questionnaire.Id);
            questionnaire.Users.Add(User);
            questionnaire = QuestionnaireRepository.Save(questionnaire);

            SendQuestionnaireResultsToEmail(questionnaireResult.Id, WebWriterV2.Properties.Settings.Default.QuestionnaireResultEmailTo);

            return true;
        }

        [AcceptVerbs("GET")]
        public bool RemoveQuestionnaireResults(long id)
        {
            if (User.UserType != UserType.Admin) {
                throw new UnauthorizedAccessException($"userId-{User.Id} try to remove RemoveQuestionnaireResults");
            }

            QuestionnaireResultRepository.Remove(id);
            return true;
        }

        [AcceptVerbs("GET")]
        public bool SendQuestionnaireResultsToEmail(long id, string email)
        {
            var questionnaireResult = QuestionnaireResultRepository.Get(id);
            var questionnaireResultEmail = new QuestionnaireResultEmail(questionnaireResult);
            EmailHelper.SendQuestionnaireResults(email, questionnaireResultEmail);
            
            return true;
        }

        [AcceptVerbs("GET")]
        public string RemoveBroken()
        {
            if (User.UserType != UserType.Admin) {
                return "Your are not admin";
            }

            var questionnaireResults = QuestionnaireResultRepository.GetAll();
            var fake = questionnaireResults.Where(x => x.User == null);
            var fakeCount = fake.Count();
            QuestionnaireResultRepository.Remove(fake);

            return $"remove {fakeCount} rows";
        }
    }
}
