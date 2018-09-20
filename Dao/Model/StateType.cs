using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dao.Model
{
    public class StateType : BaseModel
    {
        [Required]
        [Index(IsUnique = false)]
        [MaxLength(120)]//unique constraint can not be big
        public string Name { get; set; }

        public string Desc { get; set; }

        public virtual StateBasicType BasicType { get; set; }
        public virtual Book Book { get; set; }
        public virtual List<StateChange> Changes { get; set; }
        public virtual List<StateRequirement> Requirements { get; set; }
        public virtual List<StateValue> Values { get; set; }

        /// <summary>
        /// By default false. If true state can see and use onle writer
        /// </summary>
        public bool HideFromReader { get; set; }

        public virtual User Owner { get; set; }
    }
}