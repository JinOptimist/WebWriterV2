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
            LevelNumber = chapters.First().Level;
        }

        public int LevelNumber { get; set; }
        public List<FrontChapter> Chapters { get; set; }

        /// <summary>
        /// Use only for roadmap
        /// </summary>
        public List<FrontChaptersBlock> ChaptersBlock { get; set; } = new List<FrontChaptersBlock>();
    }
}
