using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class Characteristic : BaseModel
    {
        [Required]
        public virtual CharacteristicType CharacteristicType { get; set; }

        [Required]
        public long Number { get; set; }

        public RequirementType? RequirementType { get; set; }

        public Characteristic Copy()
        {
            return new Characteristic
            {
                CharacteristicType = CharacteristicType,
                Number = Number,
                RequirementType = RequirementType
            };
        }
    }
}