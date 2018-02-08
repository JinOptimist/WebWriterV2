using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class FrontBookWithChapters : FrontBook
    {
        public FrontBookWithChapters() {
        }

        public FrontBookWithChapters(Book book, bool forWriter = false, bool roadmap = false) : base(book, forWriter) {
            //book.RootChapter
            //var level

            Levels = new List<FrontChapterLevel>();

            if (roadmap)
            {
                var currentChapter = book.RootChapter;
                var currentLevel = new FrontChapterLevel();
                Levels.Add(currentLevel);
                var currentBlock = new FrontChaptersBlock();
                currentLevel.ChaptersBlock.Add(currentBlock);
                currentBlock.Chapters.Add(new FrontChapter(currentChapter));

                FillBlock(currentChapter, currentBlock, currentLevel);
            }
            else
            {
                var chaptersByLevel = book.AllChapters.GroupBy(x => x.Level);
                foreach (IEnumerable<Chapter> chapters in chaptersByLevel)
                {
                    Levels.Add(new FrontChapterLevel(chapters));
                }
            }

            
        }

        public List<FrontChapterLevel> Levels { get; set; }
        
        public override Book ToDbModel() {
            throw new Exception("Don't use it on this class");
        }

        private void FillBlock(Chapter currentChapter, FrontChaptersBlock currentBlock, FrontChapterLevel currentLevel)
        {
            if (currentChapter.LinksFromThisChapter.Count == 1)
            {
                currentChapter = currentChapter.LinksFromThisChapter.Single().To;
                currentBlock.Chapters.Add(new FrontChapter(currentChapter));
                FillBlock(currentChapter, currentBlock, currentLevel);
            }
            else if(currentChapter.LinksFromThisChapter.Count > 0) 
            {
                currentLevel = new FrontChapterLevel();
                Levels.Add(currentLevel);
                foreach (var chapter in currentChapter.LinksFromThisChapter.Select(x => x.To))
                {
                    currentBlock = new FrontChaptersBlock();
                    currentLevel.ChaptersBlock.Add(currentBlock);
                    currentBlock.Chapters.Add(new FrontChapter(chapter));
                    FillBlock(chapter, currentBlock, currentLevel);
                }
            }
        }
    }
}