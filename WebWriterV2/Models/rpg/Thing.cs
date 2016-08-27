using System.Collections.Generic;

namespace WebWriterV2.Models.rpg
{
    public class Thing
    {
        public Hero Owner { get; set; }

        public Dictionary<StatType, int> Effects { get; set; }
    }
}