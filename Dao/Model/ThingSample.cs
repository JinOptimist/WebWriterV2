using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class ThingSample : BaseModel
    {
        [Required]
        public virtual string Name { get; set; }

        public virtual string Desc { get; set; }

        public virtual Sex RequirementSex { get; set; }

        public virtual Race RequirementRace { get; set; }

        /// <summary>
        /// If true, you must destroy item after used
        /// </summary>
        [Description("Если правда, используем только OnceEffect и уничтожаем предмет")]
        public virtual bool IsUsed { get; set; }

        /// <summary>
        /// Changing while is used
        /// </summary>
        public virtual List<Characteristic> PassiveCharacteristics { get; set; }
        /// <summary>
        /// Changing while is used
        /// </summary>
        public virtual List<State> PassiveStates { get; set; }

        /// <summary>
        /// Changing when using
        /// </summary>
        public virtual List<Characteristic> UsingEffectCharacteristics { get; set; }
        /// <summary>
        /// Changing when using
        /// </summary>
        public virtual List<State> UsingEffectState { get; set; }
    }
}