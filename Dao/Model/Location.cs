using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Dao.Model
{
    public class Location : BaseModel
    {
        [Required]
        public virtual string Name { get; set; }

        public virtual string Desc { get; set; }

        [Required(ErrorMessage = "Coordinate this location")]
        //TODO change
        public virtual Point Coordinate { get; set; }

        [Description("Heroes who there are in current place")]
        public virtual List<Hero> HeroesInLocation { get; set; }

        [Description("Guild who owns current location. Can be null")]
        public virtual Guild Guild { get; set; } = null;
    }
}