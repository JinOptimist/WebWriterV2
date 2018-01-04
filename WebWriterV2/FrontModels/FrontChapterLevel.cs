using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontChapterLevel
    {
        public FrontChapterLevel()
        {
        }

        public FrontChapterLevel(IEnumerable<Chapter> chapters)
        {
            Chapters = chapters.Select(x => new FrontChapter(x)).ToList();
        }

        public List<FrontChapter> Chapters { get; set; }
    }
}
