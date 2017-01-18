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
        public int Price { get; set; }

        [Required]
        public virtual SkillSchool School { get; set; }

        public virtual List<State> SelfChanging { get; set; }
        public virtual List<State> TargetChanging { get; set; }
    }
}
