using System.Linq;
using Dal.IRepository;
using Dal.Model;

namespace Dal.Repository
{
    public class StateValueRepository : BaseRepository<StateValue>, IStateValueRepository
    {
        private readonly IStateTypeRepository _stateTypeRepository;
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