using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class StateValueRepository : BaseRepository<StateValue>, IStateValueRepository
    {
        private readonly StateTypeRepository _stateTypeRepository;
        public StateValueRepository(WriterContext db) : base(db)
        {
            _stateTypeRepository = new StateTypeRepository(db);
        }

        public void CheckAndSave(StateValue stateValue)
        {
            if (stateValue.StateType.Id == 0)
            {
                var stateType = _stateTypeRepository.GetByName(stateValue.StateType.Name);
                if (stateType != null)
                {
                    stateValue.StateType = stateType;
                }
            }

            Save(stateValue);
        }
    }
}