using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class SkillRepository : BaseRepository<Skill>, ISkillRepository
    {
        public SkillRepository(WriterContext db) : base(db)
        {
        }

        public List<Skill> GetBySchool(SkillSchool skillSchool)
        {
            return Entity.Where(x => x.School == skillSchool).ToList();
        }
    }
}