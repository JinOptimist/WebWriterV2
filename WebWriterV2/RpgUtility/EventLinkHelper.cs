using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.RpgUtility
{
    public static class EventLinkHelper
    {
        public static void FilterLink(this List<EventLinkItem> links, Hero hero)
        {
            var forRemove = new List<EventLinkItem>();
            foreach (var link in links)
            {
                var destination = link.To;
                var failBySex = destination.RequrmentSex.HasValue
                                && destination.RequrmentSex.Value != hero.Sex
                                && destination.RequrmentSex.Value != Sex.None;
                var failByRace = destination.RequrmentRace.HasValue
                                 && destination.RequrmentRace.Value != hero.Race
                                 && destination.RequrmentRace.Value != Race.None;
                var failBySkill = destination.RequrmentSkill.Any(skill => !hero.Skills.Contains(skill));
                if (failBySex || failByRace || failBySkill)
                {
                    forRemove.Add(link);
                    continue;
                }
                foreach (var characteristic in destination.RequrmentCharacteristics)
                {
                    var heroCharacteristic = hero.Characteristics.FirstOrDefault(
                        x => x.CharacteristicType == characteristic.CharacteristicType);
                    if (heroCharacteristic == null
                        || heroCharacteristic.Number < characteristic.Number)
                    {
                        forRemove.Add(link);
                    }
                }
            }

            forRemove.ForEach(x => links.Remove(x));
        }
    }
}