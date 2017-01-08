using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontCharacteristic : BaseFront<Characteristic>
    {
        public FrontCharacteristic()
        {
        }

        public FrontCharacteristic(Characteristic characteristic)
        {
            Id = characteristic.Id;
            CharacteristicType = new FrontCharacteristicType(characteristic.CharacteristicType);
            Number = characteristic.Number;
            RequirementType = new FrontEnum(characteristic.RequirementType);
        }

        public FrontCharacteristicType CharacteristicType { get; set; }

        public long Number { get; set; }

        public FrontEnum RequirementType { get; set; }

        public override Characteristic ToDbModel()
        {
            return new Characteristic
            {
                Id = Id,
                Number = Number,
                CharacteristicType = CharacteristicType.ToDbModel(),
                RequirementType = (RequirementType?)RequirementType?.Value
            };
        }
    }
}
