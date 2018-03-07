using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class FrontBookWithChaptersV2 : BaseFront<Book>
    {
        public FrontBookWithChaptersV2() {
        }

        public FrontBookWithChaptersV2(Book book) {
            //RootChapterBranch = new ChapterBranch();

            //var currentLevel = new FrontChapterLevel();
            //var currentBlock = new FrontChaptersBlock();
            //var currentChapter = book.RootChapter;

            //Levels.Add(currentLevel);
            //currentLevel.ChaptersBlock.Add(currentBlock);
            //currentBlock.Chapters.Add(new FrontChapter(currentChapter));

            //FillBlock(currentChapter, currentBlock, currentLevel);

            ContainsCycle = new GraphHelper(book).HasCycle();

            book.AllChapters.ForEach(x => x.Level = 0);
            var maxDepth = SetDepth(book.RootChapter, 1);


            FrontChapters = book.AllChapters.Select(x => new FrontChapter(x));
            
        }

        public bool ContainsCycle { get; set; }
        public List<FrontChapter> FrontChapters { get; set; }

        private int SetDepth(Chapter chapter, int depth)
        {
            var maxDepth = depth;
            if (chapter.Level < depth) {
                chapter.Level = depth;
            }

            chapter.LinksFromThisChapter.ForEach(x => {
                var childDepth = SetDepth(x.To, chapter.Level + 1);
                if (childDepth > maxDepth) {
                    maxDepth = childDepth;
                }
            });

            return maxDepth;
        }

        public override Book ToDbModel() {
            throw new NotImplementedException();
        }
    }
}