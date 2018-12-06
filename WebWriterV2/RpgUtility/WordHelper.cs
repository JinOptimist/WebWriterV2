using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dal.Model;
using WebWriterV2.FrontModels;

namespace WebWriterV2.RpgUtility
{
    public static class WordHelper
    {
        public static long GetWordCount(string text)
        {
            return text.Split(' ').Length;
        }

        public static string GenerateHtmlForDesc(string desc)
        {
            var listOfParagraph = desc.Split('\n');
            for (var i = 0; i < listOfParagraph.Length; i++) {
                if (string.IsNullOrWhiteSpace(listOfParagraph[i])) {
                    listOfParagraph[i] = "&nbsp;";
                }
                listOfParagraph[i] = $"<p>{listOfParagraph[i]}</p>";
            }
            return string.Join("\r\n", listOfParagraph);
        }
    }
}