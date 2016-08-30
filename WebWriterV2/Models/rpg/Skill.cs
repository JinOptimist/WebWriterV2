using System.Collections.Generic;

namespace WebWriterV2.Models.rpg
{
    public class Skill
    {
        public string Name { get; set; }
        public string Desc { get; set; }

        public Hero Self { get; set; }
        public Hero Target { get; set; }

        public SkillSchool School { get; set; }

        public Dictionary<StatusType, long> SelfChanging { get; set; }
        public Dictionary<StatusType, long> TargetChanging { get; set; }
    }

    public enum SkillSchool
    {
        Fire = 1,
        Cold = 2,
        Seduction = 3, // Соблазнение, Совращение
    }
}