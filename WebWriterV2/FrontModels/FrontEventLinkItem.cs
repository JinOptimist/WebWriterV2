using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontEventLinkItem : BaseFront<ChapterLinkItem>
    {
        public FrontEventLinkItem()
        {
        }

        public FrontEventLinkItem(ChapterLinkItem eventLinkItemDb)
        {
            Id = eventLinkItemDb.Id;
            Text = eventLinkItemDb.Text;
            FromId = eventLinkItemDb.From.Id;
            ToId = eventLinkItemDb.To.Id;

            HeroStatesChanging = eventLinkItemDb.HeroStatesChanging?.Select(x => new FrontState(x)).ToList();
            ThingsChanges = eventLinkItemDb.ThingsChanges?.Select(x => new FrontThing(x)).ToList();
            RequirementThings = eventLinkItemDb.RequirementThings?.Select(x => new FrontThing(x)).ToList();
            RequirementStates = eventLinkItemDb.RequirementStates?.Select(x => new FrontState(x)).ToList();
        }

        public string Text { get; set; }
        public long FromId { get; set; }
        public long ToId { get; set; }

        public List<FrontState> RequirementStates { get; set; }
        public List<FrontThing> RequirementThings { get; set; }
        public List<FrontThing> ThingsChanges { get; set; }
        public List<FrontState> HeroStatesChanging { get; set; }

        public override ChapterLinkItem ToDbModel()
        {
            return new ChapterLinkItem
            {
                Id = Id,
                Text = Text,
                From = new Chapter {Id = FromId},
                To = new Chapter {Id = ToId},

                RequirementThings = RequirementThings?.Select(x => x.ToDbModel()).ToList(),
                RequirementStates = RequirementStates?.Select(x => x.ToDbModel()).ToList(),
                ThingsChanges = ThingsChanges?.Select(x => x.ToDbModel()).ToList(),
                HeroStatesChanging = HeroStatesChanging?.Select(x => x.ToDbModel()).ToList()
            };
        }
    }
}