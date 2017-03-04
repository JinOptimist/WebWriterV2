using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dao.Model
{
    public class StateType : BaseModel
    {
        [Required]
        [Index(IsUnique = false)]
        [MaxLength(120)]//unique constraint can not be big
        public string Name { get; set; }

        public string Desc { get; set; }

        public virtual User Owner { get; set; }
    }
}