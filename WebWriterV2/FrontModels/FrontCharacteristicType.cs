using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontCharacteristicType : BaseFront<CharacteristicType>
    {
        public FrontCharacteristicType()
        {
        }

        public FrontCharacteristicType(CharacteristicType characteristicType)
        {
            Id = characteristicType.Id;
            Name = characteristicType.Name;
            Desc = characteristicType.Desc;
            State = characteristicType.EffectState.Select(x => new FrontState(x)).ToList();
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public List<FrontState> State { get; set; }

        public override CharacteristicType ToDbModel()
        {
            return new CharacteristicType
            {
                Id = Id,
                Name = Name,
                Desc = Desc,
                EffectState = State.Select(x=>x.ToDbModel()).ToList()
            };
        }
    }
}
