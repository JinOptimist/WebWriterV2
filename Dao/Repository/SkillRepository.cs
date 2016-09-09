using System.Collections.Generic;
using System.Data;
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

        public override Skill Get(long id)
        {
            return Entity.Include(x => x.SelfChanging).Include(x => x.TargetChanging).FirstOrDefault(x => x.Id == id);
        }

        public override void Save(Skill skill)
        {
            var skillByName = GetByName(skill.Name);
            if (skillByName != null && skillByName.Id != skill.Id)
            {
                throw new DuplicateNameException("Skill cann't has duplication in name");
            }

            base.Save(skill);
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