using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dao.Model;
using WebWriterV2.FrontModels;

namespace WebWriterV2.RpgUtility
{
    public static class WordHelper
    {
        public static long GetWordCount(string text)
        {
            return text.Split(' ').Length;
        }
    }
}