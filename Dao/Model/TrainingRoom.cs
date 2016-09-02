using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class TrainingRoom : BaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public long Price { get; set; }

        [Required]
        public SkillSchool School { get; set; }
    }
}