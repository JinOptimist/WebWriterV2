using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class FrontTravelIsEnded : BaseFront<Travel>
    {
        public FrontTravelIsEnded()
        {
        }

        public FrontTravelIsEnded(Travel travel)
        {
            Id = travel.Id;
            Book = new FrontBook(travel.Book);
            Steps = travel.Steps.Where(x => !string.IsNullOrWhiteSpace(x.Choice?.Text))
                .Select(x => new FrontTravelStep(x)).ToList();
            StartTime = travel.StartTime;
            FinishTime = travel.FinishTime.Value;
            TimeToRead = FinishTime - StartTime;
        }

        public FrontBook Book { get; set; }
        public List<FrontTravelStep> Steps { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public TimeSpan TimeToRead { get; set; }

        public override Travel ToDbModel()
        {
            throw new NotImplementedException();
        }
    }
}
