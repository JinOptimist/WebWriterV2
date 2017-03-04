using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontQuest : BaseFront<Quest>
    {
        public FrontQuest()
        {
        }

        public FrontQuest(Quest quest)
        {
            Id = quest.Id;
            Name = quest.Name;
            Desc = quest.Desc;
            RootEvent = quest.RootEvent != null ? new FrontEvent(quest.RootEvent) : null;
            AllEvents = quest.AllEvents?.Select(x => new FrontEvent(x)).ToList();
            OwnerId = quest.Owner?.Id;
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public long? OwnerId { get; set; }
        public FrontEvent RootEvent { get; set; }
        public List<FrontEvent> AllEvents { get; set; }

        public override Quest ToDbModel()
        {
            return new Quest
            {
                Id = Id,
                Name = Name,
                Desc = Desc,
                RootEvent = RootEvent?.ToDbModel(),
                AllEvents = AllEvents?.Select(x => x.ToDbModel()).ToList(),
                Owner = OwnerId.HasValue ? new User { Id = OwnerId.Value } : null
            };
        }
    }
}