namespace WebWriterV2.Models.rpg
{
    public class Hero
    {
        public string Name { get; set; }
        public Race Race { get; set; }
        public Sex Sex { get; set; }
        public string Background { get; set; }
    }

    public enum Race
    {
        None = 0,
        Human = 1,
        Elf = 2,
        Orc = 3,
    }

    public enum Sex
    {
        None = 0,
        Male = 1,
        Female = 2,
        Unknown = 3
    }
}