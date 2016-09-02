using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class Skill : BaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Desc { get; set; }

        public Hero Self { get; set; }
        public Hero Target { get; set; }

        [Required]
        public SkillSchool School { get; set; }

        public Dictionary<StatusType, long> SelfChanging { get; set; }
        public Dictionary<StatusType, long> TargetChanging { get; set; }
    }

    public enum SkillSchool
    {
        Fire = 1,
        Cold = 2,
        Seduction = 3, // Соблазнение, Совращение
        Base = 4, // Удар, укланени, Блок щитом
    }
}