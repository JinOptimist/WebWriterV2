using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontChapterLinkItem : BaseFront<ChapterLinkItem>
    {
        public FrontChapterLinkItem()
        {
        }

        public FrontChapterLinkItem(ChapterLinkItem eventLinkItemDb)
        {
            Id = eventLinkItemDb.Id;
            Text = eventLinkItemDb.Text;
            FromId = eventLinkItemDb.From.Id;
            ToId = eventLinkItemDb.To.Id;

            //HeroStatesChanging = eventLinkItemDb.HeroStatesChanging?.Select(x => new FrontState(x)).ToList();
            //RequirementStates = eventLinkItemDb.RequirementStates?.Select(x => new FrontState(x)).ToList();
            throw new NotImplementedException();
        }

        public string Text { get; set; }
        public long FromId { get; set; }
        public long ToId { get; set; }

        public List<FrontState> RequirementStates { get; set; }
        public List<FrontState> HeroStatesChanging { get; set; }

        public override ChapterLinkItem ToDbModel()
        {
            return new ChapterLinkItem
            {
                Id = Id,
                Text = Text,
                From = new Chapter {Id = FromId},
                To = new Chapter {Id = ToId},

                //RequirementStates = RequirementStates?.Select(x => x.ToDbModel()).ToList(),
                //HeroStatesChanging = HeroStatesChanging?.Select(x => x.ToDbModel()).ToList()
            };
            throw new NotImplementedException();
        }
    }
}