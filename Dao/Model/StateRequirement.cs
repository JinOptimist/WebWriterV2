using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class StateRequirement : BaseModel
    {
        [Required]
        public RequirementType RequirementType { get; set; }

        [Required]
        public virtual StateType StateType { get; set; }

        public virtual ChapterLinkItem Chapter { get; set; }

        /// <summary>
        /// Can be null if RequirementType is Exist or NotExist
        /// </summary>
        public long? Number { get; set; }
    }
}