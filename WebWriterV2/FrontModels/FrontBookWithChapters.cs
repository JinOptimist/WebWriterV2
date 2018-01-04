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

        public FrontBookWithChapters(Book book, bool forWriter = false) : base(book, forWriter) {
            Levels = new List<FrontChapterLevel>();
            var chaptersByLevel = book.AllChapters.GroupBy(x => x.Level);
            foreach(var chapter in chaptersByLevel) {
                Levels.Add(new FrontChapterLevel(chapter));
            }
        }

        public List<FrontChapterLevel> Levels { get; set; }
        
        public override Book ToDbModel() {
            throw new Exception("Don't use it on this class");
        }
    }
}