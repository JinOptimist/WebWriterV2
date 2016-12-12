using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontEvent : BaseFront<Event>
    {
        public FrontEvent()
        {
        }

        public FrontEvent(Event eventDb, int deep = 0)
        {
            Id = eventDb.Id;
            Name = eventDb.Name;
            Desc = eventDb.Desc;
            RequrmentRace = new FronEnum(eventDb.RequrmentRace);
            RequrmentSex = new FronEnum(eventDb.RequrmentSex);
            ChildrenEvents = deep < 2
                ? ChildrenEvents = eventDb.ChildrenEvents?.Select(x => new FrontEvent(x, ++deep)).ToList()
                : ChildrenEvents = new List<FrontEvent>();
            ProgressChanging = eventDb.ProgressChanging;
            RequrmentSkill = eventDb.RequrmentSkill?.Select(x => new FrontSkill(x)).ToList();
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public FronEnum RequrmentRace { get; set; }
        public FronEnum RequrmentSex { get; set; }
        public List<FrontEvent> ChildrenEvents { get; set; }
        public List<FrontSkill> RequrmentSkill { get; set; }
        public double ProgressChanging { get; set; }

        public override Event ToDbModel()
        {
            return new Event
            {
                Id = Id,
                Name = Name,
                Desc = Desc,
                RequrmentRace = (Race?)RequrmentRace?.Value,
                RequrmentSex = (Sex?)RequrmentSex?.Value,
                ChildrenEvents = ChildrenEvents.Select(x => x.ToDbModel()).ToList(),
                ProgressChanging = ProgressChanging,
                RequrmentSkill = RequrmentSkill?.Select(x => x.ToDbModel()).ToList()
            };
        }
    }
}