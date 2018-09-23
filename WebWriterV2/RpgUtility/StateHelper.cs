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

            foreach (var linkItem in linkItems)
            {
                if (!linkItem.StateRequirement.Any())
                {
                    result.Add(linkItem);
                    continue;
                }

                var requirements = linkItem.StateRequirement;
                var linkIsAcceptable = CheckIsLinkAcceptable(linkItem, travel.State);
                if (linkIsAcceptable)
                {
                    result.Add(linkItem);
                }
            }

            return result;
        }

        public static void ApplyChangeToTravel(Travel travel, List<StateChange> stateChanges)
        {
            var states = travel.State;

            foreach (var change in stateChanges)
            {
                var state = states.SingleOrDefault(x => x.StateType.Id == change.StateType.Id);
                if (state == null)
                {
                    state = new StateValue()
                    {
                        StateType = change.StateType,
                        Travel = travel
                    };
                    states.Add(state);
                }

                switch (change.StateType.BasicType)
                {
                    case StateBasicType.number:
                        {
                            ApplyNumberChnage(travel, state, change);
                            break;
                        }
                    case StateBasicType.boolean:
                    case StateBasicType.text:
                        {
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

            switch (change.ChangeType)
            {
                case ChangeType.Add:
                    {
                        stateValue.Value += change.Number;
                        break;
                    }
                case ChangeType.Reduce:
                    {
                        stateValue.Value -= change.Number;
                        break;
                    }
                case ChangeType.Remove:
                    {
                        travel.State.Remove(stateValue);
                        break;
                    }
                case ChangeType.Set:
                    {
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

            switch (change.ChangeType)
            {
                case ChangeType.Add:
                case ChangeType.Reduce:
                    {
                        throw new Exception("We have a problem. Some how we try add/reduce text. it's impossible");
                    }

                case ChangeType.Remove:
                    {
                        travel.State.Remove(stateValue);
                        break;
                    }
                case ChangeType.Set:
                    {
                        stateValue.Text = change.Text;
                        break;
                    }
            }
        }

        private static bool CheckIsLinkAcceptable(ChapterLinkItem linkItem, List<StateValue> travelStateValues)
        {
            foreach (var requirement in linkItem.StateRequirement)
            {
                var actualStateValue = travelStateValues.SingleOrDefault(x => x.StateType.Id == requirement.StateType.Id);

                if (actualStateValue == null)
                {
                    if (requirement.RequirementType == RequirementType.NotExist)
                        continue;
                    else
                        return false;
                }

                if (!CheckIsLinkAcceptable(actualStateValue, requirement))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool CheckIsLinkAcceptable(StateValue actualStateValue, StateRequirement requirement)
        {
            switch (requirement.StateType.BasicType)
            {
                case StateBasicType.number:
                    {
                        return CheckNumberIsLinkAcceptable(actualStateValue, requirement);
                    }
                case StateBasicType.text:
                case StateBasicType.boolean:
                    {
                        return CheckTextOrBoolIsLinkAcceptable(actualStateValue, requirement);
                    }
            }
            throw new Exception($"Uknow type of StateRequirement. StateType.Id = {requirement.StateType.Id} \r\n BasicType = {requirement.StateType.BasicType}");
        }

        private static bool CheckNumberIsLinkAcceptable(StateValue actualStateValue, StateRequirement requirement)
        {
            if (requirement.StateType.BasicType != StateBasicType.number)
                throw new Exception("CheckNumberIsLinkAcceptable get not number requirement");

            switch (requirement.RequirementType)
            {
                case RequirementType.More:
                    {
                        if (actualStateValue?.Value <= requirement.Number.Value)
                            return false;
                        break;
                    }
                case RequirementType.MoreOrEquals:
                    {
                        if (actualStateValue?.Value < requirement.Number.Value)
                            return false;
                        break;
                    }

                case RequirementType.Less:
                    {
                        if (actualStateValue?.Value >= requirement.Number.Value)
                            return false;
                        break;
                    }
                case RequirementType.LessOrEquals:
                    {
                        if (actualStateValue?.Value > requirement.Number.Value)
                            return false;
                        break;
                    }
                case RequirementType.Exist:
                    {
                        if (actualStateValue == null)
                            return false;
                        break;
                    }
                case RequirementType.NotExist:
                    {
                        if (actualStateValue != null)
                            return false;
                        break;
                    }
                case RequirementType.Equals:
                    {
                        if (actualStateValue?.Value != requirement.Number.Value)
                            return false;
                        break;
                    }
                case RequirementType.NotEquals:
                    {
                        if (actualStateValue?.Value == requirement.Number.Value)
                            return false;
                        break;
                    }
            }

            return true;
        }

        private static bool CheckTextOrBoolIsLinkAcceptable(StateValue actualStateValue, StateRequirement requirement)
        {
            if (requirement.StateType.BasicType != StateBasicType.text
                && requirement.StateType.BasicType != StateBasicType.boolean)
                throw new Exception("CheckTextOrBoolIsLinkAcceptable get not text and not boolean requirement");

            switch (requirement.RequirementType)
            {
                case RequirementType.Exist:
                    {
                        if (actualStateValue == null)
                            return false;
                        break;
                    }
                case RequirementType.NotExist:
                    {
                        if (actualStateValue != null)
                            return false;
                        break;
                    }
                case RequirementType.Equals:
                    {
                        if (actualStateValue == null || actualStateValue.Text != requirement.Text)
                            return false;
                        break;
                    }
                case RequirementType.NotEquals:
                    {
                        if (actualStateValue?.Text == requirement.Text)
                            return false;
                        break;
                    }
                default:
                    {
                        throw new Exception($"Unkown or forbidden RequirementType {requirement.RequirementType}");
                    }
            }

            return true;
        }
    }
}