using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class CharacteristicType : BaseModel
    {
        [Required]
        public string Name { get; set; }

        public string Desc { get; set; }

        public virtual List<State> EffectState { get; set; }
    }
}