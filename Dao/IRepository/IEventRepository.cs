using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        void RemoveEventAndHisChildren(Event currentEvent);
        void RemoveEventAndHisChildren(long id);
    }
}