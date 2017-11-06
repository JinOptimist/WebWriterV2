using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class StateChange : BaseModel
    {
        [Required]
        public virtual StateType StateType { get; set; }

        [Required]
        public ChangeType ChangeType { get; set; }

        /// <summary>
        /// Can be null if ChangeType is Remove
        /// </summary>
        public long? Number { get; set; }

        /// <summary>
        /// Use for store data which doesn't need to see readers
        /// </summary>
        public bool Invisible { get; set; }
    }
}