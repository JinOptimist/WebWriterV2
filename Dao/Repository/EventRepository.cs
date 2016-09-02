using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public Event GetWithParentAndChildren(long id)
        {
            return Entity.Include(x => x.ChildrenEvents).Include(x => x.ParentEvents).FirstOrDefault(x => x.Id == id);
        }

        public void RemoveEventAndHisChildren(Event currentEvent)
        {
            if (currentEvent.ChildrenEvents == null)
                currentEvent = GetWithParentAndChildren(currentEvent.Id);

            var childrenEvents = currentEvent?.ChildrenEvents.ToList();
            if (childrenEvents != null)
            {
                foreach (var childrenEvent in childrenEvents)
                {
                    childrenEvent.ParentEvents.Remove(currentEvent);
                    Save(childrenEvent);
                }
            }

            Entity.Remove(currentEvent);
            Db.SaveChanges();

            if (childrenEvents == null)
                return;
            foreach (var childrenEvent in childrenEvents)
            {
                RemoveEventAndHisChildren(childrenEvent);
            }
        }

        public void RemoveEventAndHisChildren(long id)
        {
            var currentEvent = GetWithParentAndChildren(id);
            RemoveEventAndHisChildren(currentEvent);
        }

        public List<Event> GetEvents(long questId)
        {
            return Entity.Include(x => x.ParentEvents).Where(x => x.Quest.Id == questId).ToList();
        }
    }
}