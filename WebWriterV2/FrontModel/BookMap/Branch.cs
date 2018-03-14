using Dao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebWriterV2.FrontModels;

namespace WebWriterV2.FrontModel.BookMap
{
    public class Branch: VisualBlock
    {
        public Chapter RootChapter { get; set; }

        public List<Chapter> Chapters { get; set; }
    }
}
