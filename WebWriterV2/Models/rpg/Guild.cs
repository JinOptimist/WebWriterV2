using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebWriterV2.Models.rpg
{
    public class Guild : BaseModel
    {
        public string Name { get; set; }

        public string Desc { get; set; }

        public long Gold { get; set; } = 0; // Main resource
        public long Influence { get; set; } = 0; // Second resource

        public Location Location { get; set; }

        public List<Hero> Heroes { get; set; } = new List<Hero>();
    }
}