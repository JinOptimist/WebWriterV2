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
        public virtual Race Race { get; set; }

        [Required]
        public virtual Sex Sex { get; set; }

        //[Required]
        //public Location Location { get; set; }

        public virtual List<Characteristic> Characteristics { get; set; }

        public virtual List<State> State { get; set; }

        public virtual List<Skill> Skills { get; set; }

        public virtual Guild Guild { get; set; }

        //public List<Thing> Inventory { get; set; }
    }

    public enum Race
    {
        None = -1,
        Человек = 1,
        Эльф = 2,
        Орк = 3,
        Гном = 4,
        Дракон = 5,
    }

    public enum Sex
    {
        None = -1,
        Муж = 1,
        Жен = 2,
        Скрывает = 3
    }
}