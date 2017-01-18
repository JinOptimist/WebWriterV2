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

            foreach (var chara in eventDb.CharacteristicsChanges)
            {
                var characteristic = hero.Characteristics.FirstOrDefault(x => x.CharacteristicType.Id == chara.CharacteristicType.Id);
                if (characteristic == null)
                {
                    hero.Characteristics.Add(chara.Copy());
                }
                else
                {
                    characteristic.Number += chara.Number;
                }
            }
        }

        public static void FilterLink(this List<EventLinkItem> links, Hero hero)
        {
            var forRemove = new List<EventLinkItem>();
            foreach (var link in links)
            {
                var destination = link.To;
                /* Filter by Sex */
                var failBySex = destination.RequirementSex.HasValue
                                && destination.RequirementSex.Value != hero.Sex
                                && destination.RequirementSex.Value != Sex.None;
                /* Filter by Race */
                var failByRace = destination.RequirementRace.HasValue
                                 && destination.RequirementRace.Value != hero.Race
                                 && destination.RequirementRace.Value != Race.None;
                /* Filter by Skill */
                var failBySkill = destination.RequirementSkill.Any(skill => !hero.Skills.Contains(skill));
                if (failBySex || failByRace || failBySkill)
                {
                    forRemove.Add(link);
                    continue;
                }

                /* Filter by Characteristic */
                foreach (var characteristic in destination.RequirementCharacteristics)
                {
                    var heroCharacteristic = hero.Characteristics.FirstOrDefault(
                        x => x.CharacteristicType.Id == characteristic.CharacteristicType.Id);
                    if (heroCharacteristic == null
                        || !CheckRequirement(heroCharacteristic.Number, characteristic.Number, characteristic.RequirementType))
                    {
                        forRemove.Add(link);
                    }
                }

                /* Filter by Characteristic */
                foreach (var state in destination.RequirementStates)
                {
                    var heroState = hero.State.FirstOrDefault(
                        x => x.StateType.Id == state.StateType.Id);
                    if (heroState == null
                        || !CheckRequirement(heroState.Number, state.Number, state.RequirementType))
                    {
                        forRemove.Add(link);
                    }
                }

                /* Filter by Thing */
                foreach (var thing in destination.RequirementThings)
                {
                    var heroThing = hero.Inventory.FirstOrDefault(
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