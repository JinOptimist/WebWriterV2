using Dao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class FrontChapter : BaseFront<Chapter>
    {
        public FrontChapter()
        {
        }

        public FrontChapter(Chapter chapter, Travel travel = null)
        {
            Id = chapter.Id;
            Name = chapter.Name;
            Desc = travel != null ? WordHelper.GenerateHtmlForDesc(chapter.Desc) : chapter.Desc;
            Level = chapter.Level;

            LinksFromThisChapter = travel == null
                ? chapter.LinksFromThisChapter?.Select(x => new FrontChapterLinkItem(x)).ToList()
                : travel.FilterLink(chapter.LinksFromThisChapter).Select(x => new FrontChapterLinkItem(x)).ToList();

            LinksToThisChapter = chapter.LinksToThisChapter?.Select(x => new FrontChapterLinkItem(x)).ToList();
            BookId = chapter.Book.Id;
            IsRootChapter = chapter.Book.RootChapter?.Id == chapter.Id;
            StateTypes = chapter.Book.States.Select(x => new FrontStateType(x)).ToList();

            RequirementTypes = EnumHelper.GetFrontEnumList<FrontEnumRequirementType>(typeof(RequirementType));
            //RequirementTypes = RequirementTypes.Where(x => x.Value != (int)RequirementType.Exist && x.Value != (int)RequirementType.NotExist).ToList();
            ChangeTypes = EnumHelper.GetFrontEnumList<FrontEnumChangeType>(typeof(ChangeType));
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public int Level { get; set; }
        public List<FrontChapterLinkItem> LinksFromThisChapter { get; set; }
        public List<FrontChapterLinkItem> LinksToThisChapter { get; set; }
        public bool IsRootChapter { get; set; }
        public long BookId { get; set; }

        public int Weight { get; set; } = 1;
        public int Depth { get; set; }

        public List<FrontStateType> StateTypes { get; set; }
        public List<FrontEnum> RequirementTypes { get; set; }
        public List<FrontEnum> ChangeTypes { get; set; }

        public List<long> ParentsIds { get; set; }

        public int StatisticOfVisiting { get; set; }

        public override Chapter ToDbModel()
        {
            var book = new Book() { Id = BookId };
            return new Chapter
            {
                Id = Id,
                Name = Name,
                Desc = Desc,
                Level = Level,
                LinksFromThisChapter = LinksFromThisChapter?.Select(x => x.ToDbModel()).ToList(),
                LinksToThisChapter = LinksToThisChapter?.Select(x => x.ToDbModel()).ToList(),
                NumberOfWords = WordHelper.GetWordCount(Desc),
                Book = book,
            };
        }
    }
}