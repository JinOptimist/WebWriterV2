using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontSkill : BaseFront<Skill>
    {
        public FrontSkill()
        {
        }

        public FrontSkill(Skill skill)
        {
            Id = skill.Id;
            Name = skill.Name.Replace(' ', '\u00a0');// u00a0 == &nbsp;
            Desc = skill.Desc;
            School = new FrontSkillSchool(skill.School, true);

            SelfChanging = new List<FrontState>();
            if (skill.SelfChanging != null)
            {
                foreach (var state in skill.SelfChanging)
                {
                    SelfChanging.Add(new FrontState(state));
                }
            }

            TargetChanging = new List<FrontState>();
            if (skill.TargetChanging != null)
            {
                foreach (var state in skill.TargetChanging)
                {
                    TargetChanging.Add(new FrontState(state));
                }
            }
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public List<FrontState> SelfChanging { get; set; }
        public List<FrontState> TargetChanging { get; set; }
        public FrontSkillSchool School { get; set; }
        public override Skill ToDbModel()
        {
            var skill = new Skill
            {
                Id = Id,
                Name = Name,
                Desc = Desc,
                School = School.ToDbModel(),
                SelfChanging = SelfChanging.Select(x => x.ToDbModel()).ToList(),
                TargetChanging = TargetChanging.Select(x => x.ToDbModel()).ToList(),
            };
            return skill;
        }
    }
}