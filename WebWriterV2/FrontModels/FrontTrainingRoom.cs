using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontTrainingRoom
    {
        public FrontTrainingRoom()
        {
        }

        public FrontTrainingRoom(TrainingRoom room, IEnumerable<Skill> skills)
        {
            Name = room.Name;
            School = new FronEnum(room.School);
            Skills = skills.Select(skill => new FrontSkill(skill)).ToList();
        }

        public string Name { get; set; }

        public FronEnum School { get; set; }

        public List<FrontSkill> Skills { get; set; }
    }
}