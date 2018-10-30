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
            var travels = elChel.StatisticOfVisitingAllWay();
            CountOfWays = travels.Count;
            CountOfChapters = book.AllChapters.Count;
            CountOfChoices = book.AllChapters.Count(x => x.LinksFromThisChapter.Count > 1);
            CountOfChoicesWithCondition = book.AllChapters
                .Count(chapter =>
                    chapter.LinksFromThisChapter.Any(link =>
                        link.StateChanging.Any() || link.StateRequirement.Any()
                    ));

            PercentOfStepsWithCoiceForReader =
                CountOfWays > 0
                    ? travels.Sum(travel => 100.0 * travel.Steps.Where(st => st.Choice.From.LinksFromThisChapter.Count > 1).Count() / travel.Steps.Count) / CountOfWays
                    : 0;

            AvgCountOfStepsForReader =
                CountOfWays > 0
                    ? 1.0 * travels.Sum(travel => travel.Steps.Count()) / CountOfWays
                    : 0; 

        }

        public int CountOfWays { get; set; }
        public int CountOfChoices { get; set; }
        public int CountOfChapters { get; set; }
        public int CountOfChoicesWithCondition { get; set; }
        public double AvgCountOfStepsForReader { get; set; }
        public double PercentOfStepsWithCoiceForReader { get; set; }
        

        public override Book ToDbModel()
        {
            var book = base.ToDbModel();

            return book;
        }
    }
}