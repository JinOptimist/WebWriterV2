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

        public virtual List<Characteristic> Characteristics { get; set; }

        public virtual List<State> State { get; set; }

        public virtual List<Skill> Skills { get; set; }

        public virtual Guild Guild { get; set; }

        /// <summary>
        /// Things in bag
        /// </summary>
        public virtual List<Thing> Inventory { get; set; }

        public virtual Event CurrentEvent { get; set; }

        public virtual User Owner { get; set; }
    }
}