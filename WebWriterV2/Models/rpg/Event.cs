using Dao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WebWriterV2.Models.rpg
{
    public class FrontEvent
    {
        public FrontEvent(Event eventDb)
        {
            Id = eventDb.Id;
            Name = eventDb.Name;
            Desc = eventDb.Desc;
            ChildrenEvents = eventDb.ChildrenEvents?.Select(x => new FrontEvent(x)).ToList();
            ProgressChanging = eventDb.ProgressChanging;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public List<FrontEvent> ChildrenEvents { get; set; }
        public double ProgressChanging { get; set; } = 0;
    }
}