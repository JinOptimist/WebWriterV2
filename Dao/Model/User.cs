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

        // Need to vk auth
        public string ConfirmCode { get; set; }

        [Required]
        public string Password { get; set; }

        public UserType UserType { get; set; } = UserType.Reader;

        public string AvatarUrl { get; set; }

        public virtual List<Travel> MyTravels { get; set; }

        /// <summary>
        /// Books created by User
        /// </summary>
        public virtual List<Book> Books { get; set; }

        public virtual List<UserWhoReadBook> BooksAreReaded { get; set; }

        public virtual List<StateType> StateTypes { get; set; }

        //public virtual List<ThingSample> ThingsSample { get; set; }

        /// <summary>
        /// Hero contains CurrentChapter. Use this field to get information about Book and Chapter where user stop reading
        /// </summary>
        //public virtual List<Hero> Bookmarks { get; set; }

        public virtual List<Evaluation> Evaluations { get; set; }
    }
}