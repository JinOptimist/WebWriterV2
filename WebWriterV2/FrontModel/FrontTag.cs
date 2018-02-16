using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontTag : BaseFront<Tag>
    {
        public FrontTag()
        {
        }

        public FrontTag(Tag tag)
        {
            Id = tag.Id;
            Name = tag.Name;
            BookIds = tag.Books?.Select(x => x.Id).ToList();
        }

        public string Name { get; set; }
        public List<long> BookIds { get; set; }

        public override Tag ToDbModel()
        {
            return new Tag {
                Id = Id,
                Name = Name,
            };
        }
    }
}
