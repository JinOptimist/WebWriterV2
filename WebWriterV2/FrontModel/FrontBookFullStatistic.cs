using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class FrontBookFullStatistic : FrontBook
    {
        public FrontBookFullStatistic()
        {
        }

        public FrontBookFullStatistic(Book book) : base(book, true)
        {
            var elChel = new ElChel(book);
            CountOfWays = elChel.StatisticOfVisitingAllWay();
            CountOfChapters = book.AllChapters.Count;
            CountOfChoices = book.AllChapters.Count(x => x.LinksFromThisChapter.Count > 1);
            CountOfChoicesWithCondition = book.AllChapters
                .Count(chapter => 
                    chapter.LinksFromThisChapter.Any(link => 
                        link.StateChanging.Any() || link.StateRequirement.Any()
                    ));
        }

        public int CountOfWays { get; set; }
        public int CountOfChoices { get; set; }
        public int CountOfChapters { get; set; }
        public int CountOfChoicesWithCondition { get; set; }

        public override Book ToDbModel()
        {
            var book = base.ToDbModel();

            return book;
        }
    }
}