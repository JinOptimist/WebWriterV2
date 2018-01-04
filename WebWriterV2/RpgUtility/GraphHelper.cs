using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.RpgUtility
{
    public class GraphHelper
    {
        private List<Chapter> chapters { get; set; }
        private List<long> whiteChapters {get;set;}
        private List<long> grayChapters { get; set; } = new List<long>();
        private List<long> blackChapters { get; set; } = new List<long>();

        public GraphHelper(Book book)
        {
            chapters = book.AllChapters;
            whiteChapters = chapters?.Select(x => x.Id).ToList();
        }

        public bool HasCycle()
        {
            if (chapters == null && whiteChapters == null)
                return false;

            foreach(var chapter in chapters) {
                if (IsChapterContainsCycle(chapter)) {
                    return true;
                }
            }

            return false;
        }

        private bool IsChapterContainsCycle(Chapter chapter)
        {
            //if we already check this chapter go next
            if (blackChapters.Contains(chapter.Id)) {
                return false;
            }
            //if we alreay chapter like a Gray and find them again we find cycle
            if (grayChapters.Contains(chapter.Id)) {
                return true;
            }

            whiteChapters.Remove(chapter.Id);
            if (!chapter.LinksFromThisChapter?.Any() ?? true) {
                blackChapters.Add(chapter.Id);
                return false;
            } else {
                grayChapters.Add(chapter.Id);
            }

            foreach (var link in chapter.LinksFromThisChapter) {
                if (IsChapterContainsCycle(link.To)) {
                    return true;
                }
            }

            grayChapters.Remove(chapter.Id);
            blackChapters.Add(chapter.Id);
            return false;
        }
    }
}