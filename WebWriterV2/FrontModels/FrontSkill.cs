using System.Collections.Generic;
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
            Name = skill.Name.Replace(' ', '\u00a0');
            Desc = skill.Desc;
            School = skill.School;

            SelfChanging = new List<FronEnumPlusValue>();
            if (skill.SelfChanging != null)
            {
                foreach (var state in skill.SelfChanging)
                {
                    var fronEnum = new FronEnum(state.StateType);
                    var number = state.Number;
                    SelfChanging.Add(new FronEnumPlusValue(fronEnum, number));
                }
            }

            TargetChanging = new List<FronEnumPlusValue>();
            if (skill.TargetChanging != null)
            {
                foreach (var state in skill.TargetChanging)
                {
                    var fronEnum = new FronEnum(state.StateType);
                    var number = state.Number;
                    TargetChanging.Add(new FronEnumPlusValue(fronEnum, number));
                }
            }
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public List<FronEnumPlusValue> SelfChanging { get; set; }
        public List<FronEnumPlusValue> TargetChanging { get; set; }
        public SkillSchool School { get; set; }
        public override Skill ToDbModel()
        {
            throw new System.NotImplementedException();
        }
    }
}