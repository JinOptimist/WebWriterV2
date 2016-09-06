using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class Hero : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public string Background { get; set; }

        [Required]
        public Race Race { get; set; }

        [Required]
        public Sex Sex { get; set; }

        //[Required]
        //public Location Location { get; set; }

        public List<Characteristic> Characteristics { get; set; }

        public List<State> State { get; set; }

        public List<Skill> Skills { get; set; }

        //public List<Thing> Inventory { get; set; }
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

    public enum StateType
    {
        MaxHp = 1,
        MaxMp = 2,
        Experience = 3,
        CurrentHp = 4,
        CurrentMp = 5,
        Gold = 6,
        Dodge = 7,
        Trust = 8,
    }
}