using System.Collections.Generic;
using System.Linq;
using Dal.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontGenre : BaseFront<Genre>
    {
        public FrontGenre()
        {
        }

        public FrontGenre(Genre genre)
        {
            Id = genre.Id;
            Name = genre.Name;
            Desc = genre.Desc;
        }

        public string Name { get; set; }
        public string Desc { get; set; }

        public override Genre ToDbModel()
        {
            return new Genre {
                Id = Id,
                Name = Name,
                Desc = Desc
            };
        }
    }
}
