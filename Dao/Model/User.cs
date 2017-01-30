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

        [Required]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        public List<Quest> Quests { get; set; }
    }
}