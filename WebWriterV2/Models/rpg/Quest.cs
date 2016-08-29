using System.Collections.Generic;

namespace WebWriterV2.Models.rpg
{
    public class Quest
    {
        public string Name { get; set; }
        public string Desc { get; set; }

        public double Effective { get; set; } = 0;// [1.0 = 100%]

        public Dictionary<StatType, int> Result { get; set; }

        public Hero Executor { get; set; }

        public List<Event> QuestEvents { get; set; } = new List<Event>();
        public Event CurentEvent { get; set; }
        public List<Event> History { get; set; } = new List<Event>();
    }
}
