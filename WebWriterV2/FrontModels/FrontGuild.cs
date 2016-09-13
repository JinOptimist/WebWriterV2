using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontGuild : BaseFront<Guild>
    {
        public FrontGuild()
        {
        }

        public FrontGuild(Guild guild, IDictionary<SkillSchool, List<Skill>> skillsBySchool)
        {
            Id = guild.Id;
            Name = guild.Name;
            Desc = guild.Desc;
            Gold = guild.Gold;
            Influence = guild.Influence;
            Heroes = guild.Heroes.Select(x => new FrontHero(x)).ToList();
            TrainingRooms = guild.TrainingRooms.Select(x => new FrontTrainingRoom(x, skillsBySchool[x.School])).ToList();
        }

        public string Name { get; set; }

        public string Desc { get; set; }

        public long Gold { get; set; }

        public long Influence { get; set; }

        public Location Location { get; set; }

        public List<FrontHero> Heroes { get; set; }

        public List<FrontTrainingRoom> TrainingRooms { get; set; }

        public override Guild ToDbModel()
        {
            throw new NotImplementedException();
        }
    }
}