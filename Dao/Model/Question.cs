using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Dal.Model
{
    public class Question : BaseModel
    {
        public string Text { get; set; }
        public bool AllowMultipleAnswers { get; set; }
        public int Order { get; set; }
        public virtual List<QuestionAnswer> Answers { get; set; }

        /// <summary>
        /// If true show textarea for custom answer
        /// </summary>
        public bool EnableOtherAnswer { get; set; }
        public string OtherLabel { get; set; }
        public virtual List<QuestionOtherAnswer> OtherAnswers { get; set; }

        /// <summary>
        /// Current question must be visible only if one of following answers were checked
        /// </summary>
        public virtual List<QuestionAnswer> VisibleIf { get;  set; }

        public virtual Questionnaire Questionnaire { get; set; }
    }
}
