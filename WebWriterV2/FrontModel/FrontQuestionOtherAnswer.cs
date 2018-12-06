using Dal.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontQuestionOtherAnswer : BaseFront<QuestionOtherAnswer>
    {
        public FrontQuestionOtherAnswer() {
        }

        public FrontQuestionOtherAnswer(QuestionOtherAnswer questionOtherAnswer)
        {
            AnswerText = questionOtherAnswer.AnswerText;
            QuestionId = questionOtherAnswer.Question.Id;
            QuestionnaireResultId = questionOtherAnswer.QuestionnaireResult.Id;
        }

        public string AnswerText { get; set; }
        public long QuestionId { get; set; }
        public long QuestionnaireResultId { get; set; }

        public override QuestionOtherAnswer ToDbModel()
        {
            return new QuestionOtherAnswer
            {
                Id = Id,
                AnswerText = AnswerText,
                Question = new Question { Id = QuestionId },
                QuestionnaireResult = new QuestionnaireResult { Id = QuestionnaireResultId }
            };
        }
    }
}