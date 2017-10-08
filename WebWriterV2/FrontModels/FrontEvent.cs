﻿using Dao.Model;
using System.Collections.Generic;
using System.Linq;

namespace WebWriterV2.FrontModels
{
    public class FrontEvent : BaseFront<Chapter>
    {
        public FrontEvent()
        {
        }

        public FrontEvent(Chapter eventDb)
        {
            Id = eventDb.Id;
            Name = eventDb.Name;
            Desc = eventDb.Desc;
            LinksFromThisEvent = eventDb.LinksFromThisChapter?.Select(x => new FrontEventLinkItem(x)).ToList();
            LinksToThisEvent = eventDb.LinksToThisChapter?.Select(x => new FrontEventLinkItem(x)).ToList();
            ProgressChanging = eventDb.ProgressChanging;
            HeroStatesChanging = eventDb.HeroStatesChanging?.Select(x => new FrontState(x)).ToList();
            ThingsChanges = eventDb.ThingsChanges?.Select(x => new FrontThing(x)).ToList();
            RequirementThings = eventDb.RequirementThings?.Select(x => new FrontThing(x)).ToList();
            RequirementStates = eventDb.RequirementStates?.Select(x => new FrontState(x)).ToList();

            BookId = eventDb.Book.Id;
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public List<FrontEventLinkItem> LinksFromThisEvent { get; set; }
        public List<FrontEventLinkItem> LinksToThisEvent { get; set; }
        public List<FrontState> RequirementStates { get; set; }
        public List<FrontThing> RequirementThings { get; set; }
        public List<FrontThing> ThingsChanges { get; set; }
        public List<FrontState> HeroStatesChanging { get; set; }

        public long BookId { get; set; }

        public double ProgressChanging { get; set; }

        public override Chapter ToDbModel()
        {
            return new Chapter
            {
                Id = Id,
                Name = Name,
                Desc = Desc,
                LinksFromThisChapter = LinksFromThisEvent?.Select(x => x.ToDbModel()).ToList(),
                LinksToThisChapter = LinksToThisEvent?.Select(x => x.ToDbModel()).ToList(),
                ProgressChanging = ProgressChanging,

                RequirementThings = RequirementThings?.Select(x => x.ToDbModel()).ToList(),
                RequirementStates = RequirementStates?.Select(x => x.ToDbModel()).ToList(),
                ThingsChanges = ThingsChanges?.Select(x => x.ToDbModel()).ToList(),
                HeroStatesChanging = HeroStatesChanging?.Select(x => x.ToDbModel()).ToList()
            };
        }
    }
}