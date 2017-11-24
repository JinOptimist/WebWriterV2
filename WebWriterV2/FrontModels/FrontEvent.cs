using Dao.Model;
using System.Collections.Generic;
using System.Linq;

namespace WebWriterV2.FrontModels
{
    public class FrontChapter : BaseFront<Chapter>
    {
        public FrontChapter()
        {
        }

        public FrontChapter(Chapter eventDb)
        {
            Id = eventDb.Id;
            Name = eventDb.Name;
            Desc = eventDb.Desc;
            LinksFromThisEvent = eventDb.LinksFromThisChapter?.Select(x => new FrontChapterLinkItem(x)).ToList();
            LinksToThisEvent = eventDb.LinksToThisChapter?.Select(x => new FrontChapterLinkItem(x)).ToList();

            BookId = eventDb.Book.Id;
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public List<FrontChapterLinkItem> LinksFromThisEvent { get; set; }
        public List<FrontChapterLinkItem> LinksToThisEvent { get; set; }
        

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
            };
        }
    }
}