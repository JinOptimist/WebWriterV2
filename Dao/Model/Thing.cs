using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class Thing : BaseModel
    {
        public Hero Hero { get; set; }

        public ThingSample ThingSample { get; set; }

        /// <summary>
        /// If true item in use
        /// </summary>
        public bool ItemInUse { get; set; } = false;

        public int Count { get; set; } = 1;
    }
}