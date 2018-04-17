using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class FrontTravelStep : BaseFront<TravelStep>
    {
        public FrontTravelStep()
        {
        }

        public FrontTravelStep(TravelStep travelStep)
        {
            Id = travelStep.Id;

            ChapterName = travelStep.Choice.From.Name;
            ChoiceText = travelStep.Choice.Text;
        }

        public string ChapterName { get; set; }
        public string ChoiceText { get; set; }

        public override TravelStep ToDbModel()
        {
            throw new NotImplementedException();
        }
    }
}
