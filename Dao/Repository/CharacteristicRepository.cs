using System;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class CharacteristicRepository : BaseRepository<Characteristic>, ICharacteristicRepository
    {
        private readonly Lazy<CharacteristicTypeRepository> _characteristicTypeRepository;

        public CharacteristicRepository(WriterContext db) : base(db)
        {
            _characteristicTypeRepository =
                new Lazy<CharacteristicTypeRepository>(() => new CharacteristicTypeRepository(db));
        }

        public void CheckAndSave(Characteristic characteristic)
        {
            if (characteristic.CharacteristicType.Id == 0)
            {
                var characteristicType = _characteristicTypeRepository.Value.Entity.FirstOrDefault(
                    x => x.Name == characteristic.CharacteristicType.Name);
                if (characteristicType != null)
                {
                    characteristic.CharacteristicType = characteristicType;
                }
            }

            Save(characteristic);
        }
    }
}