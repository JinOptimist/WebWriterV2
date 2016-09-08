using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Dao.Model
{
    public class Skill : BaseModel
    {
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(120)]//unique constraint can not be big
        public string Name { get; set; }

        [Required]
        public string Desc { get; set; }

        [Required]
        public SkillSchool School { get; set; }

        public List<State> SelfChanging { get; set; }
        public List<State> TargetChanging { get; set; }
    }

    public enum SkillSchool
    {
        Fire = 1,
        Cold = 2,
        Seduction = 3, // Соблазнение, Совращение
        Base = 4, // Удар, укланени, Блок щитом
    }
}