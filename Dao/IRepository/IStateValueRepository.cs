using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dal.Model;

namespace Dal.IRepository
{
    public interface IStateValueRepository : IBaseRepository<StateValue>
    {
        void CheckAndSave(StateValue stateValue);
    }
}