using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class QuestRepository : BaseRepository<Quest>, IQuestRepository
    {
        private readonly EventRepository _eventRepository;

        public QuestRepository(WriterContext db) : base(db)
        {
            _eventRepository = new EventRepository(db);
        }

        public Quest GetWithRootEvent(long id)
        {
            return Entity.Include(x => x.RootEvent).FirstOrDefault(x => x.Id == id);
        }

        public List<Quest> GetAllWithRootEvent()
        {
            return Entity.Include(x => x.RootEvent).ToList();
        }

        public override void Remove(Quest quest)
        {
            if (quest == null)
                return;

            if (quest.RootEvent == null)
                quest = GetWithRootEvent(quest.Id);

            if (quest.RootEvent != null)
            {
                _eventRepository.Remove(quest.RootEvent);
                quest = Get(quest.Id);
            }

            base.Remove(quest);
        }

        public override void Remove(long id)
        {
            var quest = GetWithRootEvent(id);
            Remove(quest);
        }
    }
}