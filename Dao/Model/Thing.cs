using System.Collections.Generic;

namespace Dao.Model
{
    public class Thing : BaseModel
    {
        public Hero Owner { get; set; }

        public Dictionary<CharacteristicType, int> Effects { get; set; }
    }
}