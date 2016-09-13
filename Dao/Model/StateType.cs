using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class StateType : BaseModel
    {
        [Required]
        public string Name { get; set; }

        public string Desc { get; set; }
    }
}