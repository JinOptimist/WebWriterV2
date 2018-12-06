using System;
using System.Collections.Generic;
using System.Linq;
using Dal.Model;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class FrontQuestionnaireResult : BaseFront<QuestionnaireResult>
    {
        public FrontQuestionnaireResult()
        {
        }

        public FrontQuestionnaireResult(QuestionnaireResult questionnaireResult)
        {
            Id = questionnaireResult.Id;
            QuestionnaireId = questionnaireResult.Questionnaire.Id;
            UserId = questionnaireResult.User.Id;
            CreationDate = questionnaireResult.CreationDate.ToLongDateString();
            QuestionAnswers = questionnaireResult.QuestionAnswers.Select(x => new FrontQuestionAnswer(x)).ToList();

            QuestionOtherAnswers = questionnaireResult.QuestionOtherAnswers.Select(x => new FrontQuestionOtherAnswer(x)).ToList();
        }

        public long QuestionnaireId { get; set; }
        public long UserId { get; set; }
        public string CreationDate { get; set; }
        public List<FrontQuestionAnswer> QuestionAnswers { get; set; }

        public List<FrontQuestionOtherAnswer> QuestionOtherAnswers { get; set; }

        public override QuestionnaireResult ToDbModel()
        {
            return new QuestionnaireResult
            {
                Id = Id,
                Questionnaire = new Questionnaire { Id = QuestionnaireId },
                CreationDate = DateTime.Now,
                User = new User { Id = UserId },
                QuestionAnswers = QuestionAnswers.Select(x => x.ToDbModel()).ToList(),
                QuestionOtherAnswers = QuestionOtherAnswers?.Select(x => x.ToDbModel()).ToList()
            };
        }
    }
}
