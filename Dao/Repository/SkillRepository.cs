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

        public override void Save(Skill skill)
        {
            if (skill.Id > 0 && Exist(skill))
            {
                //if we try update existin skill
                base.Save(skill);
            }
            //ignore if we try add new skill with skill
        }

        public override bool Exist(Skill skill)
        {
            return Entity.Any(x => x.Name == skill.Name);
        }

        public Skill GetByName(string skillName)
        {
            return Entity.FirstOrDefault(x => x.Name == skillName);
        }

        public List<Skill> GetBySchool(SkillSchool skillSchool)
        {
            return Entity.Where(x => x.School == skillSchool).ToList();
        }

        public Dictionary<SkillSchool, List<Skill>> GetBySchools(List<SkillSchool> skillSchool)
        {
            var result = new Dictionary<SkillSchool, List<Skill>>();

            foreach (var school in skillSchool)
            {
                var skills = GetBySchool(school);
                result.Add(school, skills);
            }

            return result;
        }
    }
}