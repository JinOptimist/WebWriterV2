using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Dal.Model
{
    public class Questionnaire : BaseModel
    {
        public string Name { get; set; }

        public virtual List<Question> Questions { get; set; }

        /// <summary>
        /// If true we must show the Questionnaire for writer before he start work at first book
        /// </summary>
        public bool ShowBeforeFirstBook { get; set; }

        /// <summary>
        /// List of users who has already finished this questionnaire
        /// </summary>
        public virtual List<User> Users { get; set; }

        public virtual List<QuestionnaireResult> QuestionnaireResults{ get; set; }

    }
}
