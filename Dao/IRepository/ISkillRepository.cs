using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dao.Model;

namespace Dao.IRepository
{
    public interface ISkillRepository : IBaseRepository<Skill>
    {
        List<Skill> GetBySchool(SkillSchool skillSchool);
    }
}