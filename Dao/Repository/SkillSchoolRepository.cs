using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class SkillSchoolRepository : BaseRepository<SkillSchool>, ISkillSchoolRepository
    {
        public SkillSchoolRepository(WriterContext db) : base(db)
        {
        }

        public SkillSchool GetByName(string name)
        {
            return Entity.FirstOrDefault(x => x.Name == name);
        }
    }
}