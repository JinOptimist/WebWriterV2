using System.Collections.Generic;

namespace WebWriterV2.Models.rpg
{
    public class Quest
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        //public Effective Effective { get; set; }
        public int Progress { get; set; } = 0;// [1, 100]
        public List<PartOfQuest> PartsOfQuest { get; set; }
    }

    public class PartOfQuest
    {
        public string Desc { get; set; }
        //public Race PerfectRace { get; set; }
        //public Sex PerfectSex { get; set; }
        public List<Event> Events { get; set; }
    }
}