using System;
using System.Collections.Generic;
using System.Linq;
using Dal.Model;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class FrontQuestionnaire : BaseFront<Questionnaire>
    {
        public FrontQuestionnaire()
        {
        }

        public FrontQuestionnaire(Questionnaire questionnaire)
        {
            Id = questionnaire.Id;
            Name = questionnaire.Name;
            Questions = questionnaire.Questions.Select(x => new FrontQuestion(x)).ToList();
            HowManyTimesUserAnswerToTheQuestionnaire = questionnaire.QuestionnaireResults?.Count ?? 0;
        }

        public string Name { get; set; }
        public List<FrontQuestion> Questions { get; set; }
        public int HowManyTimesUserAnswerToTheQuestionnaire { get; set; }

        public override Questionnaire ToDbModel()
        {
            return new Questionnaire {
                Id = Id,
                Name = Name,
                Questions = Questions.Select(x => x.ToDbModel()).ToList()
            };
        }
    }
}
