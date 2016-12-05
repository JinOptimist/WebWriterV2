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
                _eventRepository.RemoveWholeBranch(quest.RootEvent);
                quest = Get(quest.Id);
            }

            base.Remove(quest);
        }

        public override void Remove(long id)
        {
            var quest = GetWithRootEvent(id);
            Remove(quest);
        }

        public override Quest Save(Quest model)
        {
            var find = Entity.Find(model.Id);
            if (find != null)
            {
                //TODO May be better move this method to generic repository?
                //find.Update(model);

                model = find;
            }
            else
            {
                if (model.Id > 0)
                    Entity.Attach(model);
                else
                    Entity.Add(model);
            }

            return base.Save(model);
        }
    }
}