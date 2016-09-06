using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dao.Model;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class FrontSkill
    {
        public FrontSkill()
        {
        }

        public FrontSkill(Skill skill)
        {
            Name = skill.Name;
            Desc = skill.Desc;

            School = EnumHelper.GetFronEnum(typeof (SkillSchool), (long) skill.School);

            SelfChanging = new List<FronEnumPlusValue>();
            if (skill.SelfChanging != null)
            {
                foreach (var state in skill.SelfChanging)
                {
                    var fronEnum = EnumHelper.GetFronEnum(typeof (StateType), (long) state.StateType);
                    var number = state.Number;
                    SelfChanging.Add(new FronEnumPlusValue(fronEnum, number));
                }
            }

            TargetChanging = new List<FronEnumPlusValue>();
            if (skill.TargetChanging != null)
            {
                foreach (var state in skill.TargetChanging)
                {
                    var fronEnum = EnumHelper.GetFronEnum(typeof(StateType), (long)state.StateType);
                    var number = state.Number;
                    TargetChanging.Add(new FronEnumPlusValue(fronEnum, number));
                }
            }
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        // название статус значения и степень её изменения
        // пример
        // мана -4
        public List<FronEnumPlusValue> SelfChanging { get; set; }
        //жизни -6
        public List<FronEnumPlusValue> TargetChanging { get; set; }

        public FronEnum School { get; set; }
    }
}