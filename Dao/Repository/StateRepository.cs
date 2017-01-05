using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class StateRepository : BaseRepository<State>, IStateRepository
    {
        private readonly StateTypeRepository _stateTypeRepository;
        public StateRepository(WriterContext db) : base(db)
        {
            _stateTypeRepository = new StateTypeRepository(db);
        }


        public void CheckAndSave(State state)
        {
            if (state.StateType.Id == 0)
            {
                var stateType = _stateTypeRepository.Entity.FirstOrDefault(x => x.Name == state.StateType.Name);
                if (stateType != null)
                {
                    state.StateType = stateType;
                }
            }

            Save(state);
        }
    }
}