using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class Thing : BaseModel
    {
        [Required]
        public string Name { get; set; }

        public string Desc { get; set; }

        public Hero Owner { get; set; }

        [Description("Если правда, используем только OnceEffect и уничтожаем предмет")]
        public bool IsUsed { get; set; }

        public List<Characteristic> Changing { get; set; }

        public List<State> OnceEffect { get; set; }
    }
}