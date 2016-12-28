using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class State : BaseModel
    {
        [Required]
        public virtual StateType StateType { get; set; }

        [Required]
        public long Number { get; set; }

        public State Copy()
        {
            return new State
            {
                StateType = StateType,
                Number = Number
            };
        }
    }

    //public enum StateType
    //{
    //    MaxHp = 1,
    //    MaxMp = 2,
    //    Experience = 3,
    //    CurrentHp = 4,
    //    CurrentMp = 5,
    //    Gold = 6,
    //    Dodge = 7,
    //    Trust = 8,
    //}
}