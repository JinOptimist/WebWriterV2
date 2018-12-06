using System.Collections.Generic;
using System.Linq;
using Dal.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontStateRequirement : BaseFront<StateRequirement>
    {
        public FrontStateRequirement()
        {
        }

        public FrontStateRequirement(StateRequirement stateRequirement)
        {
            Id = stateRequirement.Id;
            ChapterLinkId = stateRequirement.ChapterLink.Id;
            Value = stateRequirement.Number?.ToString() ?? stateRequirement.Text;
            RequirementType = new FrontEnumRequirementType(stateRequirement.RequirementType);
            StateType = new FrontStateType(stateRequirement.StateType);
        }

        public long ChapterLinkId { get; set; }
        public FrontEnum RequirementType { get; set; }
        public FrontStateType StateType { get; set; }
        public string Value { get; set; }

        public override StateRequirement ToDbModel()
        {
            var stateRequirement = new StateRequirement {
                Id = Id,
                ChapterLink = new ChapterLinkItem { Id = ChapterLinkId },
                StateType = StateType.ToDbModel(),
                RequirementType = (RequirementType)RequirementType.Value
            };
            if (Value == null)
                return stateRequirement;

            switch (StateType.BasicType) {
                case StateBasicType.Boolean: {
                        // convert to single format
                        stateRequirement.Text = bool.Parse(Value).ToString();
                        break;
                    }
                case StateBasicType.Number: {
                        stateRequirement.Number = int.Parse(Value);
                        break;
                    }
                case StateBasicType.Text: {
                        stateRequirement.Text = Value;
                        break;
                    }
            }

            return stateRequirement;
        }
    }
}
