using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontThing : BaseFront<Thing>
    {
        public FrontThing()
        {
        }

        public FrontThing(Thing thing)
        {
            Id = thing.Id;
            ItemInUse = thing.ItemInUse;
            Count = thing.Count;
            ThingSample = new FrontThingSample(thing.ThingSample);
            RequirementType = new FrontEnum(thing.RequirementType);
        }

        public FrontThingSample ThingSample { get; set; }
        public bool ItemInUse { get; set; }
        public int Count { get; set; }
        public FrontEnum RequirementType { get; set; }

        public override Thing ToDbModel()
        {
            return new Thing
            {
                Id = Id,
                ItemInUse = ItemInUse,
                ThingSample = ThingSample.ToDbModel(),
                Count = Count,
                RequirementType = (RequirementType?)RequirementType?.Value
            };
        }
    }
}
