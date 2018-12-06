using System.Linq;
using Dal.IRepository;
using Dal.Model;

namespace Dal.Repository
{
    public class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        public TagRepository(WriterContext db) : base(db)
        {
        }

        public Tag GetOrCreate(string tagName)
        {
            var tag = Entity.SingleOrDefault(x => x.Name == tagName);
            if (tag == null) {
                tag = new Tag {
                    Name = tagName
                };
                tag = Save(tag);
            }

            return tag;
        }
    }
}