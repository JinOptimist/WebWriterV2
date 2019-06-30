using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dal.Model
{
    public class Like : BaseModel
    {
        public virtual User User { get; set; }
        public virtual Book Book { get; set; }
        public virtual DateTime Time { get; set; }
    }
}
