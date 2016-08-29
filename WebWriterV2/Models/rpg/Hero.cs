using System.Collections.Generic;

namespace WebWriterV2.Models.rpg
{
    public class Hero
    {
        public string Name { get; set; }
        public Race Race { get; set; }
        public Sex Sex { get; set; }
        public string Background { get; set; }

        public Location Location { get; set; } = null;

        public Dictionary<CharacteristicType, long> Characteristics { get; set; }
        public Dictionary<StatusType, long> Status { get; set; }
        public List<Thing> Inventory { get; set; }
    }

    public enum Race
    {
        Human = 1,
        Elf = 2,
        Orc = 3,
        Gnom = 4,
        Dragon = 5,
    }

    public enum Sex
    {
        Male = 1,
        Female = 2,
        Unknown = 3
    }

    public enum CharacteristicType
    {
        Strength = 1,
        Agility = 2,
        Charism = 3
    }

    public enum StatusType
    {
        MaxHp = 1,
        MaxMp = 2,
        Experience = 3,
        CurrentHp = 4,
        CurrentMp = 5,
        Gold = 6
    }
}