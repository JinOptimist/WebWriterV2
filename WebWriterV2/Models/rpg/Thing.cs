using System.Collections.Generic;

namespace WebWriterV2.Models.rpg
{
    public class Thing
    {
        public Hero Owner { get; set; }

        public Dictionary<CharacteristicType, int> Effects { get; set; }
    }
}