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

        public FrontChapter(Chapter chapter)
        {
            Id = chapter.Id;
            Name = chapter.Name;
            Desc = chapter.Desc;
            LinksFromThisEvent = chapter.LinksFromThisChapter?.Select(x => new FrontChapterLinkItem(x)).ToList();
            LinksToThisEvent = chapter.LinksToThisChapter?.Select(x => new FrontChapterLinkItem(x)).ToList();

            BookId = chapter.Book.Id;
            IsRootChapter = chapter.Book.RootChapter?.Id == chapter.Id;
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public List<FrontChapterLinkItem> LinksFromThisEvent { get; set; }
        public List<FrontChapterLinkItem> LinksToThisEvent { get; set; }

        public bool IsRootChapter { get; set; }

        public long BookId { get; set; }
        
        public override Chapter ToDbModel()
        {
            var book = new Book() { Id = BookId };
            return new Chapter
            {
                Id = Id,
                Name = Name,
                Desc = Desc,
                LinksFromThisChapter = LinksFromThisEvent?.Select(x => x.ToDbModel()).ToList(),
                LinksToThisChapter = LinksToThisEvent?.Select(x => x.ToDbModel()).ToList(),
                NumberOfWords = WordHelper.GetWordCount(Desc),
                Book = book,
            };
        }
    }
}