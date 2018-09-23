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

            Changes = chapterLinkItem.StateChanging?.Select(x => new FrontStateChange(x)).ToList() ?? new List<FrontStateChange>();
            Requirements = chapterLinkItem.StateRequirement?.Select(x => new FrontStateRequirement(x)).ToList() ?? new List<FrontStateRequirement>();

            // TODO Remove old code
            Decision = chapterLinkItem.StateChanging?.FirstOrDefault()?.Text;
            Condition = chapterLinkItem.StateRequirement?.FirstOrDefault()?.Text;
        }

        public string Text { get; set; }
        public long FromId { get; set; }
        public string FromChapterName { get; set; }
        public long ToId { get; set; }
        public string ToChapterName { get; set; }

        public List<FrontStateChange> Changes { get; set; }
        public List<FrontStateRequirement> Requirements { get; set; }

        // TODO Remove old code
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