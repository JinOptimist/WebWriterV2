using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Dal.Model
{
    public class QuestionnaireResult : BaseModel
    {
        public virtual Questionnaire Questionnaire { get; set; }

        public virtual User User { get; set; }

        public virtual DateTime CreationDate { get; set; }

        public virtual List<QuestionAnswer> QuestionAnswers { get; set; }

        public virtual List<QuestionOtherAnswer> QuestionOtherAnswers { get; set; }

    }
}
