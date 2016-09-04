using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class QuestRepository : BaseRepository<Quest>, IQuestRepository
    {
        private EventRepository EventRepository;

        public QuestRepository(WriterContext db) : base(db)
        {
            EventRepository = new EventRepository(db);
        }
        
        public Quest GetWithRootEvent(long id)
        {
            return Entity.Include(x => x.RootEvent).FirstOrDefault(x => x.Id == id);
        }

        public new void Remove(Quest quest)
        {
            if (quest == null)
                return;

            if (quest.RootEvent == null)
                quest = GetWithRootEvent(quest.Id);

            if (quest.RootEvent != null)
            {
                EventRepository.Remove(quest.RootEvent);
                quest = Get(quest.Id);
            }

            base.Remove(quest);
        }

        public new void Remove(long id)
        {
            var quest = GetWithRootEvent(id);
            Remove(quest);
        }
    }
}