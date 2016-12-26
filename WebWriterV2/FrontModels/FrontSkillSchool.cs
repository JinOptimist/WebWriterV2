using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontSkillSchool : BaseFront<SkillSchool>
    {
        public FrontSkillSchool()
        {
        }

        public FrontSkillSchool(SkillSchool school, bool whitoutSkills = false)
        {
            Id = school.Id;
            Name = school.Name;
            Desc = school.Desc;
            Skills = whitoutSkills
                ? new List<FrontSkill>()
                : school.Skills.Select(x => new FrontSkill(x)).ToList();
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public List<FrontSkill> Skills { get; set; }

        public override SkillSchool ToDbModel()
        {
            var school = new SkillSchool
            {
                Id = Id,
                Name = Name,
                Desc = Desc,
                Skills = Skills.Select(x => x.ToDbModel()).ToList()
            };
            return school;
        }
    }
}