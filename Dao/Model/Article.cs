using Dal.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Dal.Model
{
    public class Article : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(300)]
        public string ShortDesc { get; set; }
        [Required]
        public string Desc { get; set; }

        public bool IsPublished { get; set; }
        public int Views { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
