using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebWriterV2.Models.rpg
{
    public class Location
    {
        public string Name { get; set; }
        public string Desc { get; set; }

        public Point Coordinate { get; set; }

        public List<Hero> HeroesInLocation { get; set; } = new List<Hero>();
    }
}