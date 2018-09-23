using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontStateChange : BaseFront<StateChange>
    {
        public FrontStateChange()
        {
        }

        public FrontStateChange(StateChange stateChange)
        {
            Id = stateChange.Id;
            ChapterLinkId = stateChange.ChapterLink.Id;
            Value = stateChange.Number?.ToString() ?? stateChange.Text;
            ChangeType = new FrontEnumChangeType(stateChange.ChangeType);
            StateType = new FrontStateType(stateChange.StateType);
        }

        public long ChapterLinkId { get; set; }
        public FrontEnum ChangeType { get; set; }
        public FrontStateType StateType { get; set; }
        public string Value { get; set; }

        public override StateChange ToDbModel()
        {
            var stateChange = new StateChange {
                Id = Id,
                ChapterLink = new ChapterLinkItem { Id = ChapterLinkId },
                StateType = StateType.ToDbModel(),
                ChangeType = (ChangeType)ChangeType.Value
            };
            if (Value == null)
                return stateChange;

            switch (StateType.BasicType) {
                case StateBasicType.Boolean: {
                        // convert to single format
                        stateChange.Text = bool.Parse(Value).ToString();
                        break;
                    }
                case StateBasicType.Number: {
                        stateChange.Number = int.Parse(Value);
                        break;
                    }
                case StateBasicType.Text: {
                        stateChange.Text = Value;
                        break;
                    }
            }

            return stateChange;
        }
    }
}
