using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class State : BaseModel
    {
        [Required]
        public virtual StateType StateType { get; set; }

        [Required]
        public virtual long Number { get; set; }
    }
}