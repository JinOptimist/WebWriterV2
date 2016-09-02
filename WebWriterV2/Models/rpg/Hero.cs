using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebWriterV2.Models.rpg
{
    public class Hero : BaseModel
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
        Человек = 1,
        Эльф = 2,
        Орк = 3,
        Гном = 4,
        Дракон = 5,
    }

    public enum Sex
    {
        Муж = 1,
        Жен = 2,
        Скрывает = 3
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
        Gold = 6,
        Dodge = 7,
    }
}