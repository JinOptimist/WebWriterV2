using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.RpgUtility
{
    public static class EventHelper
    {
        public static void EventChangesApply(this Event eventDb, Hero hero)
        {
            foreach (var thing in eventDb.ThingsChanges)
            {
                hero.Inventory = hero.Inventory ?? new List<Thing>();
                var heroThing = hero.Inventory.FirstOrDefault(x => x.ThingSample.Id == thing.ThingSample.Id);
                if (heroThing == null)
                {
                    var newThing = thing.Copy();
                    newThing.Hero = hero;
                    hero.Inventory.Add(newThing);
                }
                else
                {
                    heroThing.Count += thing.Count;
                }
            }

            foreach (var state in eventDb.HeroStatesChanging)
            {
                var heroState = hero.State.FirstOrDefault(x => x.StateType.Id == state.StateType.Id);
                if (heroState == null)
                {
                    hero.State.Add(state.Copy());
                }
                else
                {
                    heroState.Number += state.Number;
                }
            }
        }

        public static void FilterLink(this List<EventLinkItem> links, Hero hero)
        {
            var forRemove = new List<EventLinkItem>();
            foreach (var link in links)
            {
                var destination = link.To;
                /* Filter by States */
                foreach (var stateRequirement in destination.RequirementStates) {
                    var heroState = hero.State.FirstOrDefault(x => x.StateType.Id == stateRequirement.StateType.Id);

                    if (stateRequirement.RequirementType == RequirementType.NotEquals && heroState == null) {
                        // it's normal when RequirementType is NotEquals and current hero hasn't this state
                        // therefore skip checking
                    }
                    else
                    {
                        if (heroState == null || !CheckRequirement(heroState.Number, stateRequirement.Number, stateRequirement.RequirementType)) 
                        {
                            forRemove.Add(link);
                        }
                    }
                }

                /* Filter by Thing */
                foreach (var thing in destination.RequirementThings)
                {
                    var heroThing = hero.Inventory?.FirstOrDefault(
                        x => x.ThingSample.Id == thing.ThingSample.Id);
                    if (heroThing == null
                        || !CheckRequirement(heroThing.Count, thing.Count, thing.RequirementType))
                    {
                        forRemove.Add(link);
                    }
                }
            }

            forRemove.ForEach(x => links.Remove(x));
        }

        private static bool CheckRequirement(long heroValue, long requirementValue, RequirementType? requirementType)
        {
            if (requirementType == null)
            {
                return true;
            }

            switch (requirementType)
            {
                case RequirementType.Equals:
                    {
                        return heroValue == requirementValue;
                    }
                case RequirementType.NotEquals:
                    {
                        return heroValue != requirementValue;
                    }
                case RequirementType.LessOrEquals:
                    {
                        return heroValue <= requirementValue;
                    }
                case RequirementType.MoreOrEquals:
                    {
                        return heroValue >= requirementValue;
                    }
                case RequirementType.Less:
                    {
                        return heroValue < requirementValue;
                    }
                case RequirementType.More:
                    {
                        return heroValue > requirementValue;
                    }
                case RequirementType.Exist:
                    {
                        return heroValue > 0;
                    }
                case RequirementType.NotExist:
                    {
                        return heroValue < 1;
                    }
                default:
                    {
                        throw new Exception("Uknown RequirementType");
                    }
            }

        }
    }
}