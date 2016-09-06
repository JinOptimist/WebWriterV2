using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class State : BaseModel
    {
        [Required]
        public StateType StateType { get; set; }

        [Required]
        public long Number { get; set; }
    }
}