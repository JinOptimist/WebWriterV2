using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class Characteristic : BaseModel
    {
        [Required]
        public CharacteristicType CharacteristicType { get; set; }

        [Required]
        public long Number { get; set; }
    }
}