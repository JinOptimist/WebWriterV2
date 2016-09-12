using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontTrainingRoom:BaseFront<TrainingRoom>
    {
        public FrontTrainingRoom()
        {
        }

        public FrontTrainingRoom(TrainingRoom room, IEnumerable<Skill> skills)
        {
            Id = room.Id;
            Name = room.Name;
            School = room.School;
            Skills = skills.Select(skill => new FrontSkill(skill)).ToList();
        }

        public string Name { get; set; }

        public SkillSchool School { get; set; }

        public List<FrontSkill> Skills { get; set; }

        public override TrainingRoom ToDbModel()
        {
            return new TrainingRoom();
        }
    }
}