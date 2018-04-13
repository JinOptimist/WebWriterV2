using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class FrontTravel : BaseFront<Travel>
    {
        public FrontTravel()
        {
        }

        public FrontTravel(Travel travel) : this(travel, travel.Steps?.LastOrDefault()?.Choice?.To ?? travel.Book.RootChapter) { }

        public FrontTravel(Travel travel, Chapter chapter)
        {
            Id = travel.Id;
            if (travel.Book != null) {
                Book = new FrontBook(travel.Book);
            }
            Chapter = new FrontChapter(chapter, travel);

            AllStates = travel.State == null ? "" : string.Join(", ", travel.State);

            var hasCycle = new GraphHelper(travel.Book).HasCycle();
            if (hasCycle) {
                return;
            }
            var stepFromWhichReaderCame = travel.Steps?.SingleOrDefault(x => x.Choice.To.Id == chapter.Id);
            PrevChapterId = stepFromWhichReaderCame?.Choice.From.Id;
            var currentStep = travel.Steps?.SingleOrDefault(x => x.Choice.From.Id == chapter.Id);
            NextChapterId = currentStep?.Choice.To.Id;
        }

        public FrontBook Book { get; set; }
        public FrontChapter Chapter { get; set; }
        public string AllStates { get; set; }

        public long? PrevChapterId { get; set; }
        public long? NextChapterId { get; set; }

        public override Travel ToDbModel()
        {
            throw new NotImplementedException();
        }
    }
}
