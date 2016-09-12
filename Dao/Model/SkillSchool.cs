using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Dao.Model
{
    public class SkillSchool : BaseModel
    {
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(120)]//unique constraint can not be big
        public string Name { get; set; }

        public string Desc { get; set; }
    }
}
