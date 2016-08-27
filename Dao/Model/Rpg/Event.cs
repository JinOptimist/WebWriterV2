namespace Dao.Model.Rpg
{
    public class Event
    {
        public Sex RequrmentSex { get; set; } = Sex.None;
        public Race RequrmentRace { get; set; } = Race.None;
        public string Desc { get; set; }
        public int ProgressChanging { get; set; } = 0;
        public int HeroMoneyChanging { get; set; } = 0;
        public int HeroHpChanging { get; set; } = 0;
        public double HeroLvlChanging { get; set; } = 0;
    }
}