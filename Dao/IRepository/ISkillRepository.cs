using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dao.Model;

namespace Dao.IRepository
{
    public interface ISkillRepository : IBaseRepository<Skill>
    {
        Skill GetByName(string skillName);

        List<Skill> GetBySchool(SkillSchool skillSchool);

        List<Skill> GetBySchoolName(string schoolName);

        Dictionary<SkillSchool, List<Skill>> GetBySchools(List<SkillSchool> skillSchool);

        Skill CheckAndSave(Skill skill);
    }
}