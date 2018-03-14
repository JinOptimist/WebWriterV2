using Dao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebWriterV2.FrontModels;

namespace WebWriterV2.FrontModel.BookMap
{
    public class VisualBlock
    {
        public int Width { get; set; } = 1;

        public int Depth { get; set; }

        public List<VisualBlock> Child { get; set; }
    }
}
