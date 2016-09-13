﻿using System;
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
            RootEvent = new FrontEvent(quest.RootEvent);
            AllEvents = quest.AllEvents.Select(x => new FrontEvent(x)).ToList();
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public FrontEvent RootEvent { get; set; }
        public List<FrontEvent> AllEvents { get; set; }

        public override Quest ToDbModel()
        {
            throw new NotImplementedException();
        }
    }
}