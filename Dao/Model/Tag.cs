using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Dal.Model
{
    public class Tag : BaseModel
    {
        [Index(IsUnique = true), StringLength(500)]
        public string Name { get; set; }

        public virtual List<Book> Books { get; set; }
    }
}
