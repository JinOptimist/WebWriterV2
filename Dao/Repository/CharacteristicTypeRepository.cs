using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class CharacteristicTypeRepository : BaseRepository<CharacteristicType>, ICharacteristicTypeRepository
    {
        private readonly IStateTypeRepository _stateTypeRepository;
        private readonly CharacteristicRepository _characteristicRepository;

        public CharacteristicTypeRepository(WriterContext db) : base(db)
        {
            _stateTypeRepository = new StateTypeRepository(db);
            _characteristicRepository = new CharacteristicRepository(db);
        }

        public override CharacteristicType Save(CharacteristicType model)
        {
            model.EffectState = model.EffectState ?? new List<State>();
            foreach (var state in model.EffectState)
            {
                var isDetached = _stateTypeRepository.Db.Entry(state.StateType).State == EntityState.Detached;
                if (isDetached)
                {
                    state.StateType = new StateType { Id = state.StateType.Id };
                    _stateTypeRepository.Entity.Attach(state.StateType);
                }
            }

            return base.Save(model);
        }

        public override void Remove(CharacteristicType baseModel)
        {
            var characteristics = _characteristicRepository.Entity.Where(x => x.CharacteristicType.Id == baseModel.Id).ToList();
            _characteristicRepository.Remove(characteristics);
            base.Remove(baseModel);
        }
    }
}