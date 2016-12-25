using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.RpgUtility
{
    public static class ThingHelper
    {
        public static void AddThingToHero(this Hero hero, ThingSample thingSample)
        {
            AddThingToHero(hero, thingSample, 1, false);
        }

        public static void AddThingToHero(this Hero hero, ThingSample thingSample, int count)
        {
            AddThingToHero(hero, thingSample, count, false);
        }

        public static void AddThingToHero(this Hero hero, ThingSample thingSample, bool itemInUse)
        {
            AddThingToHero(hero, thingSample, 1, itemInUse);
        }

        private static void AddThingToHero(this Hero hero, ThingSample thingSample, int count, bool itemInUse)
        {
            if (hero.Inventory == null)
            {
                hero.Inventory = new List<Thing>();
            }

            var thing = new Thing
            {
                Count = count,
                Hero = hero,
                ThingSample = thingSample,
                ItemInUse = itemInUse
            };

            hero.Inventory.Add(thing);
        }
    }
}