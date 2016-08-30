using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebWriterV2.Models.rpg
{
    public class Thing : BaseModel
    {
        public Hero Owner { get; set; }

        public Dictionary<CharacteristicType, int> Effects { get; set; }
    }
}