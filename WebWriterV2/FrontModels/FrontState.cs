using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontState : BaseFront<StateValue>
    {
        public FrontState()
        {
        }

        public FrontState(StateValue state)
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
