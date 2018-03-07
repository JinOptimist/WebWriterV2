using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebWriterV2.FrontModels;

namespace WebWriterV2.FrontModel.BookMap
{
    public class ChapterBranch
    {
        public int MaxInnerCountOnSingleDepth { get; set; } = 0;

        /// <summary>
        /// List of chapters without any choose
        /// </summary>
        public List<FrontChapter> FrontChapters { get; set; }

        //public ChapterBranch Parent
        public List<ChapterBranch> ChildBranches { get; set; }
    }
}
