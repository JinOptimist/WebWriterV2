using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class FrontQuestionAnswer : BaseFront<QuestionAnswer>
    {
        public FrontQuestionAnswer()
        {
        }

        public FrontQuestionAnswer(QuestionAnswer questionAnswer)
        {
            Id = questionAnswer.Id;
            QuestionId = questionAnswer.Question.Id;
            Text = questionAnswer.Text;
            Order = questionAnswer.Order;
            HowManyTimesWasChoosen = questionAnswer.QuestionnaireResults?.Count ?? 0;
        }

        public long QuestionId { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public int HowManyTimesWasChoosen { get; set; }

        public override QuestionAnswer ToDbModel()
        {
            return new QuestionAnswer {
                Id = Id,
                Text = Text,
                Order = Order,
                Question = new Question { Id = QuestionId },
            };
        }
    }
}
