using System.Collections.Generic;

namespace Dao.Model.Rpg
{
    public class Thing
    {
        public Hero Owner { get; set; }

        public Dictionary<StatType, int> Effects { get; set; }
    }
}