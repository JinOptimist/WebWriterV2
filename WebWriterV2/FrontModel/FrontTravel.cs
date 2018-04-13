using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontTravel : BaseFront<Travel>
    {
        public FrontTravel()
        {
        }

        public FrontTravel(Travel travel, long chapterId = -1)
        {
            Id = travel.Id;
            if (travel.Book != null) {
                Book = new FrontBook(travel.Book);
            }
            if (travel.CurrentChapter != null) {
                Chapter = new FrontChapter(travel.CurrentChapter, travel);
            }

            var stepFromWhichReaderCame = travel.Steps?.SingleOrDefault(x => x.Choice.To.Id == chapterId);
            PrevChapterId = stepFromWhichReaderCame?.Choice.From.Id;
            var currentStep = travel.Steps?.SingleOrDefault(x => x.Choice.From.Id == chapterId);

            if (currentStep != null) {
                NextChapterId = currentStep.Choice.To.Id;
                Chapter = new FrontChapter(currentStep.Choice.From, travel);
            }

            AllStates = travel.State == null ? "" : string.Join(", ", travel.State);
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
