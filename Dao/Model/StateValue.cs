using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dao.Model
{
    public class StateValue : BaseModel
    {
        [Required]
        public virtual StateType StateType { get; set; }

        public virtual Travel Travel { get; set; }

        public long? Value { get; set; }

        public string Text { get; set; }
    }
}