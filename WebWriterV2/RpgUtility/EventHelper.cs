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
                        || heroCharacteristic.Number < characteristic.Number)
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
                        || heroThing.Count < thing.Count)
                    {
                        forRemove.Add(link);
                    }
                }
            }

            forRemove.ForEach(x => links.Remove(x));
        }
    }
}