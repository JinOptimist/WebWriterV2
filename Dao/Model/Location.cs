using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Dao.Model
{
    public class Location : BaseModel
    {
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string Desc { get; set; }

        [Required(ErrorMessage = "Coordinate this location")]
        public Point Coordinate { get; set; }

        [Description("Heroes who there are in current place")]
        public List<Hero> HeroesInLocation { get; set; } = new List<Hero>();

        [Description("Guild who owns current location. Can be null")]
        public Guild Guild { get; set; } = null;
    }
}