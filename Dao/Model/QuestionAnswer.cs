using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Dao.Model
{
    public class QuestionAnswer : BaseModel
    {
        public string Text { get; set; }

        public int Order { get; set; }

        public virtual Question Question { get; set; }

        public virtual List<Question> AffectVisibilityOfQuestions { get; set; }

        public virtual List<QuestionnaireResult> QuestionnaireResults { get; set; }
    }
}
