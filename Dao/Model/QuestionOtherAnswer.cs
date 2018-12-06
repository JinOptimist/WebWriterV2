using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Dal.Model
{
    public class QuestionOtherAnswer : BaseModel
    {
        public string AnswerText { get; set; }

        public virtual Question Question { get; set; }

        public virtual QuestionnaireResult QuestionnaireResult { get; set; }
    }
}
