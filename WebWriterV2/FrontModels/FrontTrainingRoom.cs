using System;
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

        public FrontTrainingRoom(TrainingRoom room)
        {
            Id = room.Id;
            Name = room.Name;
            Price = room.Price;
            School = new FrontSkillSchool(room.School);
            //Skills = room.School.Skills.Select(skill => new FrontSkill(skill)).ToList();
        }

        public string Name { get; set; }
        public long Price { get; set; }        
        public FrontSkillSchool School { get; set; }
        public List<FrontSkill> Skills { get; set; }

        public override TrainingRoom ToDbModel()
        {
            throw new NotImplementedException();
        }
    }
}