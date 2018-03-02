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

        public FrontChapterLinkItem(ChapterLinkItem chapterLinkItem)
        {
            Id = chapterLinkItem.Id;
            Text = chapterLinkItem.Text;
            FromId = chapterLinkItem.From.Id;
            FromChapterName = chapterLinkItem.From.Name;
            ToId = chapterLinkItem.To.Id;
            ToChapterName = chapterLinkItem.To.Name;

            Decision = chapterLinkItem.StateChanging?.FirstOrDefault()?.Text;
            Condition = chapterLinkItem.StateRequirement?.FirstOrDefault()?.Text;
            //RequirementStates = eventLinkItemDb.RequirementStates?.Select(x => new FrontState(x)).ToList();
        }

        public string Text { get; set; }
        public long FromId { get; set; }
        public string FromChapterName { get; set; }
        public long ToId { get; set; }
        public string ToChapterName { get; set; }

        public string Condition { get; set; }
        public string Decision { get; set; }

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
        }
    }
}