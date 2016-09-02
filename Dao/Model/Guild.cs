using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class Guild : BaseModel
    {
        [Required]
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

        public List<Hero> Heroes { get; set; }

        public List<TrainingRoom> TrainingRooms { get; set; }
    }
}