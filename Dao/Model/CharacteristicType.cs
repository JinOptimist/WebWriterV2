using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dao.Model
{
    public class CharacteristicType : BaseModel
    {
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(120)]//unique constraint can not be big
        public string Name { get; set; }

        public string Desc { get; set; }

        public virtual List<State> EffectState { get; set; }
    }
}