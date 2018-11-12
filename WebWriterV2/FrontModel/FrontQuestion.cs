using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class FrontQuestion : BaseFront<Question>
    {
        public FrontQuestion()
        {
        }

        public FrontQuestion(Question question)
        {
            Id = question.Id;
            QuestionnaireId = question.Questionnaire.Id;
            Text = question.Text;
            Order = question.Order;
            AllowMultipleAnswers = question.AllowMultipleAnswers;
            VisibleIf = question.VisibleIf.Select(x => x.Id).ToList();
            QuestionAnswers = question.Answers.Select(x => new FrontQuestionAnswer(x)).ToList();
        }

        public long QuestionnaireId { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public bool AllowMultipleAnswers { get; set; }
        public List<long> VisibleIf { get; set; }
        public List<FrontQuestionAnswer> QuestionAnswers { get; set; }

        public override Question ToDbModel()
        {
            return new Question {
                Id = Id,
                Text = Text,
                Order = Order,
                VisibleIf = VisibleIf?.Select(x => new QuestionAnswer { Id = x }).ToList(),
                Answers = QuestionAnswers?.Select(x => x.ToDbModel()).ToList(),
                Questionnaire = new Questionnaire { Id = QuestionnaireId },
                AllowMultipleAnswers = AllowMultipleAnswers
            };
        }
    }
}
