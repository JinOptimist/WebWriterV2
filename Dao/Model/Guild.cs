using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class Guild : BaseModel
    {
        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Desc { get; set; }

        [Required]
        [Description("Main resource")]
        public virtual long Gold { get; set; }

        [Required]
        [Description("Second resource")]
        public virtual long Influence { get; set; }

        [Required]
        public Location Location { get; set; }

        public virtual List<Hero> Heroes { get; set; }

        public virtual List<TrainingRoom> TrainingRooms { get; set; }
    }
}