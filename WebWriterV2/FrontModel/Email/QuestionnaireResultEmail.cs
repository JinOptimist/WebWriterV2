using Dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebWriterV2.FrontModel.Email
{
    public class QuestionnaireResultEmail
    {
        public QuestionnaireResultEmail(QuestionnaireResult questionnaireResult)
        {
            QuestionnaireName = questionnaireResult.Questionnaire.Name;
            UserName = questionnaireResult.User.Name;

            QuestionAnswerPairs = new List<QuestionAnswerPairEmail>();

            foreach (var question in questionnaireResult.Questionnaire.Questions) {
                var questionAnswerPair = new QuestionAnswerPairEmail();
                questionAnswerPair.QuestionText = question.Text;
                questionAnswerPair.AnswersText = questionnaireResult
                    .QuestionAnswers.Where(x => x.Question.Id == question.Id)
                    .Select(x => x.Text).ToList();
                questionAnswerPair.OtherAnswerText = questionnaireResult.QuestionOtherAnswers.FirstOrDefault(x => x.Question.Id == question.Id)?.AnswerText;
                QuestionAnswerPairs.Add(questionAnswerPair);
            }
        }

        public string QuestionnaireName { get; set; }
        public string UserName { get; set; }
        public List<QuestionAnswerPairEmail> QuestionAnswerPairs { get;set;}
    }
}