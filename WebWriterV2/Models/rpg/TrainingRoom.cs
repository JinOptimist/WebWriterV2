using System;
using System.Runtime.Serialization;

namespace WebWriterV2.Models.rpg
{
    public class TrainingRoom : BaseModel
    {
        public string Name { get; set; }

        public long Price { get; set; }

        public SkillSchool School { get; set; }
    }
}