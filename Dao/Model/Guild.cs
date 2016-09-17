using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dao.Model
{
    public class Guild : BaseModel
    {
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(120)]//unique constraint can not be big
        public string Name { get; set; }

        [Required]
        public string Desc { get; set; }

        [Required]
        [Description("Main resource")]
        public long Gold { get; set; }

        [Required]
        [Description("Second resource")]
        public long Influence { get; set; }

        [Required]
        public Location Location { get; set; }

        public virtual List<Hero> Heroes { get; set; }

        public virtual List<TrainingRoom> TrainingRooms { get; set; }
    }
}