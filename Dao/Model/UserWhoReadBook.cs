using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dal.Model
{
    public class UserWhoReadBook : BaseModel
    {
        public virtual User User { get; set; }
        public virtual Book Book { get; set; }
    }
}

