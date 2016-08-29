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

        public Dictionary<StatType, long> Stats { get; set; }
        public Dictionary<StatusType, long> Status { get; set; }
        public List<Thing> Inventory { get; set; }
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

    public enum StatType
    {
        Strength = 1,
        Agility = 2,
        Charism = 3
    }

    public enum StatusType
    {
        Hp = 1,
        Mh = 2,
        Experience = 3,
        HandAttackPower = 4,
        HandAttackSpeed = 5
    }
}