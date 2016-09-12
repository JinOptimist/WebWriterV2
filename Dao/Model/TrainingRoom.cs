using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class TrainingRoom : BaseModel
    {
        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual long Price { get; set; }

        [Required]
        public virtual SkillSchool School { get; set; }
    }
}
