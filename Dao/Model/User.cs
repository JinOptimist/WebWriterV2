using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dao.Model
{
    public class User : BaseModel
    {
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(120)] //unique constraint can not be big
        public string Name { get; set; }

        public string Email { get; set; }

        public string ConfirmCode { get; set; }

        [Required]
        public string Password { get; set; }

        public UserType UserType { get; set; } = UserType.Reader;

        /// <summary>
        /// Hero contains CurrentEvent. Use this field to get information about Quest and Event where user stop reading
        /// </summary>
        public virtual List<Hero> Bookmarks { get; set; }

        /// <summary>
        /// Quests created by User
        /// </summary>
        public virtual List<Quest> Quests { get; set; }

        public virtual List<Quest> BooksAreReaded { get; set; }

        public virtual List<StateType> StateTypes { get; set; }

        public virtual List<ThingSample> ThingsSample { get; set; }

        public virtual List<Evaluation> Evaluations { get; set; }
    }
}