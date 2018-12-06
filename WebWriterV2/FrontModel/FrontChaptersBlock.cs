using System;
using System.Collections.Generic;
using System.Linq;
using Dal.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontChaptersBlock
    {
        public FrontChaptersBlock() {
        }

        public FrontChaptersBlock(IEnumerable<Chapter> chapters){
            Chapters = chapters.Select(x => new FrontChapter(x)).ToList();
        }

        public List<FrontChapter> Chapters { get; set; } = new List<FrontChapter>();
    }
}