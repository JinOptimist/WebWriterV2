using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dal.Model
{
    public class BookComment : BaseModel
    {
        [Required]
        public string Text { get; set; }
        public virtual DateTime PublicationDate { get; set; }

        public virtual User Author { get; set; }
        public virtual Book Book { get; set; }
    }
}

