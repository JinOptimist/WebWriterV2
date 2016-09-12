using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class Thing : BaseModel
    {
        [Required]
        public virtual string Name { get; set; }

        public virtual string Desc { get; set; }

        public virtual Hero Owner { get; set; }

        [Description("Если правда, используем только OnceEffect и уничтожаем предмет")]
        public virtual bool IsUsed { get; set; }

        public virtual List<Characteristic> Changing { get; set; }

        public virtual List<State> OnceEffect { get; set; }
    }
}