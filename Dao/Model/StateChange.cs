using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class StateChange : BaseModel
    {
        [Required]
        public ChangeType ChangeType { get; set; }

        [Required]
        public virtual StateType StateType { get; set; }

        public virtual ChapterLinkItem Chapter { get; set; }

        /// <summary>
        /// Can be null if ChangeType is Remove
        /// </summary>
        public long? Number { get; set; }
    }
}