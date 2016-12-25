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
            RequrmentRace = new FronEnum(thingSample.RequrmentRace);
            RequrmentSex = new FronEnum(thingSample.RequrmentSex);
            PassiveStates = thingSample.PassiveStates?.Select(x => new FrontState(x)).ToList();
            PassiveCharacteristics = thingSample.PassiveCharacteristics?.Select(x => new FrontCharacteristic(x)).ToList();
            UsingEffectState = thingSample.UsingEffectState?.Select(x => new FrontState(x)).ToList();
            UsingEffectCharacteristics = thingSample.UsingEffectCharacteristics?.Select(x => new FrontCharacteristic(x)).ToList();
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public bool IsUsed { get; set; }
        public FronEnum RequrmentRace { get; set; }
        public FronEnum RequrmentSex { get; set; }

        public List<FrontState> PassiveStates { get; set; }
        public List<FrontCharacteristic> PassiveCharacteristics { get; set; }
        public List<FrontState> UsingEffectState { get; set; }
        public List<FrontCharacteristic> UsingEffectCharacteristics { get; set; }

        public override ThingSample ToDbModel()
        {
            return new ThingSample
            {
                Id = Id,
                Name = Name,
                Desc = Desc,
                IsUsed = IsUsed,
                RequrmentRace = (Race)RequrmentRace.Value,
                RequrmentSex = (Sex)RequrmentSex.Value,

                PassiveStates = PassiveStates.Select(x=>x.ToDbModel()).ToList(),
                PassiveCharacteristics = PassiveCharacteristics.Select(x => x.ToDbModel()).ToList(),
                UsingEffectState = UsingEffectState.Select(x => x.ToDbModel()).ToList(),
                UsingEffectCharacteristics = UsingEffectCharacteristics.Select(x => x.ToDbModel()).ToList(),
            };
        }
    }
}
