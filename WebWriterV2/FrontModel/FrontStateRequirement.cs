using System.Collections.Generic;
using System.Linq;
using Dao.Model;

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
            RequirementType = new FrontEnum(stateRequirement.RequirementType);
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
            switch (StateType.BasicType) {
                case StateBasicType.boolean: {
                        // convert to single format
                        stateRequirement.Text = bool.Parse(Value).ToString();
                        break;
                    }
                case StateBasicType.number: {
                        stateRequirement.Number = int.Parse(Value);
                        break;
                    }
                case StateBasicType.text: {
                        stateRequirement.Text = Value;
                        break;
                    }
            }

            return stateRequirement;
        }
    }
}
