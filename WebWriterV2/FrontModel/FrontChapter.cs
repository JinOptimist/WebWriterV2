using Dao.Model;
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
            Desc = travel != null ? GenerateHtmlForDesc(chapter.Desc) : chapter.Desc;
            Level = chapter.Level;

            LinksFromThisChapter = travel == null
                ? chapter.LinksFromThisChapter?.Select(x => new FrontChapterLinkItem(x)).ToList()
                : travel.FilterLink(chapter.LinksFromThisChapter).Select(x => new FrontChapterLinkItem(x)).ToList();

            LinksToThisChapter = chapter.LinksToThisChapter?.Select(x => new FrontChapterLinkItem(x)).ToList();
            BookId = chapter.Book.Id;
            IsRootChapter = chapter.Book.RootChapter?.Id == chapter.Id;
        }

        private string GenerateHtmlForDesc(string desc)
        {
            var listOfParagraph = desc.Split('\n');
            for (var i = 0; i < listOfParagraph.Length; i++) {
                if (string.IsNullOrWhiteSpace(listOfParagraph[i])) {
                    listOfParagraph[i] = "&nbsp;";
                }
                listOfParagraph[i] = $"<p>{listOfParagraph[i]}</p>";
            }
            return string.Join("\r\n", listOfParagraph);
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

        public List<long> ParentsIds { get; set; }

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