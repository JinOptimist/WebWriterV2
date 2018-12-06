using System.ComponentModel.DataAnnotations;

namespace Dal.Model
{
    public class StateChange : BaseModel
    {
        [Required]
        public ChangeType ChangeType { get; set; }

        [Required]
        public virtual StateType StateType { get; set; }

        public virtual ChapterLinkItem ChapterLink { get; set; }

        /// <summary>
        /// Can be null if ChangeType is Remove
        /// </summary>
        public long? Number { get; set; }

        public string Text { get; set; }
    }
}