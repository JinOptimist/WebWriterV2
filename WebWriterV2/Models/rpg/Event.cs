namespace WebWriterV2.Models.rpg
{
    public class Event
    {
        public Sex RequrmentSex { get; set; } = Sex.None;
        public Race RequrmentRace { get; set; } = Race.None;
        public string Desc { get; set; }
        public int ProgressPlus { get; set; }
    }
}