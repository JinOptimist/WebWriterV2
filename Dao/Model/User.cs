using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dal.Model
{
    public class User : BaseModel
    {
        [Required]
        [MaxLength(120)]
        public string Name { get; set; }

        public string Email { get; set; }

        // Need to vk auth
        public string ConfirmCode { get; set; }

        [Required]
        public string Password { get; set; }

        public UserType UserType { get; set; } = UserType.Reader;

        public bool ShowExtendedFunctionality { get; set; }

        public string AvatarUrl { get; set; }

        public virtual List<Travel> MyTravels { get; set; }

        /// <summary>
        /// Books created by User
        /// </summary>
        public virtual List<Book> Books { get; set; }
        public virtual List<Book> AvailableButNotMineBooks { get; set; }

        public virtual List<UserWhoReadBook> BooksAreReaded { get; set; }

        public virtual List<StateType> StateTypes { get; set; }

        //public virtual List<ThingSample> ThingsSample { get; set; }

        /// <summary>
        /// Hero contains CurrentChapter. Use this field to get information about Book and Chapter where user stop reading
        /// </summary>
        //public virtual List<Hero> Bookmarks { get; set; }

        public virtual List<Evaluation> Evaluations { get; set; }

        /// <summary>
        /// List of questionnaires which user has already finished
        /// </summary>
        public virtual List<Questionnaire> Questionnaires { get; set; }
        public virtual List<QuestionnaireResult> QuestionnaireResults { get; set; }
    }
}