using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebWriterV2.Models.rpg
{
    public class Guild
    {
        public string Name { get; set; }

        public string Desc { get; set; }

        public long Influence { get; set; } = 0;

        public Location Location { get; set; }

        public List<Hero> Heroes { get; set; } = new List<Hero>();
    }
}