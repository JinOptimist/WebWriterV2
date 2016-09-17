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
    }

    //public enum CharacteristicType
    //{
    //    Strength = 1,
    //    Agility = 2,
    //    Charism = 3
    //}
}