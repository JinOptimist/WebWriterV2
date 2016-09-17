using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dao.Model
{
    public class TrainingRoom : BaseModel
    {
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(120)]//unique constraint can not be big
        public virtual string Name { get; set; }

        [Required]
        public virtual long Price { get; set; }

        [Required]
        public virtual SkillSchool School { get; set; }
    }
}
