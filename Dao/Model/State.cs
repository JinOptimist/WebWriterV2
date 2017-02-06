using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class State : BaseModel
    {
        [Required]
        public virtual StateType StateType { get; set; }

        [Required]
        public long Number { get; set; }

        /// <summary>
        /// Use for store data which doesn't need to see readers
        /// </summary>
        public bool Invisible { get; set; }

        public RequirementType? RequirementType { get; set; }

        public State Copy()
        {
            return new State
            {
                StateType = StateType,
                Number = Number,
                RequirementType = RequirementType
            };
        }
    }
}