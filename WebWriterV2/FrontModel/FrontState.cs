using System.Collections.Generic;
using System.Linq;
using Dal.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontStateValue : BaseFront<StateValue>
    {
        public FrontStateValue()
        {
        }

        public FrontStateValue(StateValue state)
        {
            Id = state.Id;
            Value = state.Value;
            StateType = new FrontStateType(state.StateType);
        }

        public long? Value { get; set; }

        public FrontStateType StateType { get; set; }

        public override StateValue ToDbModel()
        {
            return new StateValue
            {
                Id = Id,
                Value = Value,
                StateType = StateType.ToDbModel(),
            };
        }
    }
}
