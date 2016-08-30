using System.Collections.Generic;
using System.Drawing;

namespace WebWriterV2.Models.rpg
{
    public class Location : BaseModel
    {
        public string Name { get; set; }
        public string Desc { get; set; }

        public Point Coordinate { get; set; }

        public List<Hero> HeroesInLocation { get; set; } = new List<Hero>();

        public Guild Guild { get; set; } = null;
    }
}