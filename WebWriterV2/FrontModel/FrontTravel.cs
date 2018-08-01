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

        public FrontTravel(Travel travel) : this(travel, travel.CurrentStep) { }

        public FrontTravel(Travel travel, TravelStep step)
        {
            Id = travel.Id;
            if (travel.Book != null) {
                Book = new FrontBook(travel.Book);
            }

            Chapter = new FrontChapter(step.CurrentChapter, travel);
            PrevStepId = step.PrevStep?.Id;
            NextStepId = step.NextStep?.Id;
            CurrentStepId = travel.CurrentStep.Id;


            AllStates = travel.State == null ? "" : string.Join(", ", travel.State.Select(x => x.ToString()));

            CountOfUniqVisitedChapter = travel.Steps.Select(x => x.CurrentChapter.Id).Distinct().Count();
        }

        public FrontBook Book { get; set; }

        public FrontChapter Chapter { get; set; }
        public string AllStates { get; set; }

        public long CurrentStepId { get; set; }
        public long? PrevStepId { get; set; }
        public long? NextStepId { get; set; }

        public int CountOfUniqVisitedChapter { get; set; }

        public override Travel ToDbModel()
        {
            throw new NotImplementedException();
        }
    }
}
