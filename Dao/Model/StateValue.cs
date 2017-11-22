using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dao.Model
{
    public class StateValue : BaseModel
    {
        [Required]
        public virtual StateType StateType { get; set; }

        public virtual Hero Hero { get; set; }

        public long? Value { get; set; }
    }
}