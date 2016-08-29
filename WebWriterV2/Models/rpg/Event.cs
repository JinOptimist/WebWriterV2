using System.Collections.Generic;

namespace WebWriterV2.Models.rpg
{
    public class Event
    {
        public string Desc { get; set; }

        public Sex RequrmentSex { get; set; } = Sex.None;
        public Race RequrmentRace { get; set; } = Race.None;
        public Location RequrmentLocation { get; set; }
        public Dictionary<StatType, long> RequrmentStats { get; set; }

        public double ProgressChanging { get; set; } = 0;
        public Dictionary<StatusType, long> StatsChanging { get; set; }
    }
}