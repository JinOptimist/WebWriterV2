using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class QuestRepository : BaseRepository<Quest>, IQuestRepository
    {
        public Quest GetWithRootEvent(long id)
        {
            return Entity.Include(x => x.RootEvent).FirstOrDefault(x => x.Id == id);
        }
    }
}