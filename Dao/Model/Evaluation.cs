﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dao.Model
{
    public class Evaluation : BaseModel
    {
        public virtual long Mark { get; set; }

        public virtual string Comment { get; set; }

        public virtual DateTime Created { get; set; }

        public virtual User Owner { get; set; }

        public virtual Quest Quest { get; set; }
    }
}