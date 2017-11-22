using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class Hero : BaseModel
    {
        public virtual List<StateValue> State { get; set; }

        public virtual Chapter CurrentChapter { get; set; }

        public virtual User Owner { get; set; }

        public virtual DateTime TimeCreation { get; set; }

        public virtual DateTime LastChanges { get; set; }
    }
}