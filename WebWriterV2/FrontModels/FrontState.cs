using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontState : BaseFront<State>
    {
        public FrontState()
        {
        }

        public FrontState(State state)
        {
            Id = state.Id;
            Number = state.Number;
            StateType = state.StateType;
            RequirementType = new FrontEnum(state.RequirementType);
        }

        public long Number { get; set; }

        public StateType StateType { get; set; }

        public FrontEnum RequirementType { get; set; }

        public override State ToDbModel()
        {
            return new State
            {
                Id = Id,
                Number = Number,
                StateType = StateType,
                RequirementType = (RequirementType?)RequirementType?.Value
            };
        }
    }
}
