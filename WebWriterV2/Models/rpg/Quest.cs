using System.Collections.Generic;

namespace WebWriterV2.Models.rpg
{
    public class Quest
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public int Progress { get; set; } = 0;// [1, 100]
        public List<Wile> Wiles { get; set; } = new List<Wile>();
        public List<Event> EventsHistory { get; set; } = new List<Event>();
    }

    public class Wile
    {
        public string Desc { get; set; }
        public List<Event> Events { get; set; }
    }
}