using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontThingSample : BaseFront<ThingSample>
    {
        public FrontThingSample()
        {
        }

        public FrontThingSample(ThingSample thingSample)
        {
            Id = thingSample.Id;
            Name = thingSample.Name;
            Desc = thingSample.Desc;
            IsUsed = thingSample.IsUsed;
            PassiveStates = thingSample.PassiveStates?.Select(x => new FrontState(x)).ToList();
            UsingEffectState = thingSample.UsingEffectState?.Select(x => new FrontState(x)).ToList();

            OwnerId = thingSample.Owner?.Id;
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public long? OwnerId { get; set; }
        public bool IsUsed { get; set; }

        public List<FrontState> PassiveStates { get; set; }
        public List<FrontState> UsingEffectState { get; set; }

        public override ThingSample ToDbModel()
        {
            return new ThingSample
            {
                Id = Id,
                Name = Name,
                Desc = Desc,
                IsUsed = IsUsed,
                Owner = OwnerId.HasValue ? new User { Id = OwnerId.Value } : null,

                PassiveStates = PassiveStates.Select(x => x.ToDbModel()).ToList(),
                UsingEffectState = UsingEffectState.Select(x => x.ToDbModel()).ToList(),
            };
        }
    }
}
