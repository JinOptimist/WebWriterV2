using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;
using Dao.DataGeneration;

namespace WebWriterV2.RpgUtility
{
    public static class StateHelper
    {
        public static List<ChapterLinkItem> FilterLink(this Travel travel, List<ChapterLinkItem> linkItems)
        {
            var result = new List<ChapterLinkItem>();

            foreach (var linkItem in linkItems) {
                if (!linkItem.StateRequirement.Any()) {
                    result.Add(linkItem);
                    continue;
                }

                //var condition = linkItem.StateRequirement.SingleOrDefault()?.Text;
                //if (travel.State?.Any(x => x.Text == condition) ?? false) {
                //    result.Add(linkItem);
                //}

                var requirements = linkItem.StateRequirement;
                var linkIsAcceptable = CheckLink(linkItem, travel.State);
                if (linkIsAcceptable) {
                    result.Add(linkItem);
                }
            }

            return result;
        }

        public static void ApplyChangeToTravel(Travel travel, List<StateChange> stateChanges)
        {
            var states = travel.State;

            foreach (var change in stateChanges) {
                var state = states.SingleOrDefault(x => x.StateType.Id == change.StateType.Id);
                if (state == null) {
                    state = new StateValue() {
                        StateType = change.StateType,
                        Travel = travel
                    };
                    states.Add(state);
                }

                switch (change.StateType.BasicType) {
                    case StateBasicType.number: {
                            ApplyNumberChnage(travel, state, change);
                            break;
                        }
                    case StateBasicType.boolean:
                    case StateBasicType.text: {
                            ApplyTextChnage(travel, state, change);
                            break;
                        }
                }
            }
        }

        private static void ApplyNumberChnage(Travel travel, StateValue stateValue, StateChange change)
        {
            if (change.StateType.BasicType != StateBasicType.number)
                throw new Exception("Use this method only for StateBasicType.number");
            if (!stateValue.Value.HasValue)
                stateValue.Value = 0;

            switch (change.ChangeType) {
                case ChangeType.Add: {
                        stateValue.Value += change.Number;
                        break;
                    }
                case ChangeType.Reduce: {
                        stateValue.Value -= change.Number;
                        break;
                    }
                case ChangeType.Remove: {
                        travel.State.Remove(stateValue);
                        break;
                    }
                case ChangeType.Set: {
                        stateValue.Value = change.Number;
                        break;
                    }
            }
        }

        private static void ApplyTextChnage(Travel travel, StateValue stateValue, StateChange change)
        {
            if (change.StateType.BasicType != StateBasicType.text
                && change.StateType.BasicType != StateBasicType.boolean)
                throw new Exception("Use this method only for StateBasicType.text");

            switch (change.ChangeType) {
                case ChangeType.Add: 
                case ChangeType.Reduce: {
                        throw new Exception("We have a problem. Some how we try add/reduce text. it's impossible");
                    }

                case ChangeType.Remove: {
                        travel.State.Remove(stateValue);
                        break;
                    }
                case ChangeType.Set: {
                        stateValue.Text = change.Text;
                        break;
                    }
            }
        }

        private static bool CheckLink(ChapterLinkItem linkItem, List<StateValue> stateValues)
        {

            return false;
        }
    }
}