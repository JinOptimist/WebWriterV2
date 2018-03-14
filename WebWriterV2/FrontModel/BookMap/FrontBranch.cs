using Dao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebWriterV2.FrontModels;

namespace WebWriterV2.FrontModel.BookMap
{
    public class FrontBranch : VisualBlock
    {
        public FrontChapter RootChapter { get; set; }

        public List<FrontChapter> Chapters { get; set; }
    }
}
