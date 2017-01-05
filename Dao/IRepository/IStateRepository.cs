using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IStateRepository : IBaseRepository<State>
    {
        void CheckAndSave(State state);
    }
}