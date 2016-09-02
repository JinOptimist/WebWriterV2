using System.Collections.Generic;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public void RemoveEventAndHisChildren(Event currentEvent)
        {
            var a = currentEvent.ChildrenEvents;
            var b = currentEvent.ParentEvents;
            var l1 = Entity.Where(x => x.ParentEvents != null && x.ParentEvents.Contains(currentEvent));
            var l2 = Entity.Where(x => x.ChildrenEvents != null && x.ChildrenEvents.Contains(currentEvent));

            var childrenEvents = new List<Event>();
            if (currentEvent.ChildrenEvents != null)
                foreach (var childrenEvent in currentEvent.ChildrenEvents)
                    childrenEvent.ParentEvents.Remove(currentEvent);

            foreach (var childrenEvent in childrenEvents)
            {
                childrenEvent.ParentEvents.Remove(currentEvent);
                Save(childrenEvent);
            }

            Db.SaveChanges();

            currentEvent.ParentEvents = null;
            Db.SaveChanges();

            Entity.Remove(currentEvent);
            Db.SaveChanges();

            foreach (var childrenEvent in childrenEvents)
            {
                RemoveEventAndHisChildren(childrenEvent);
            }
        }

        public void RemoveEventAndHisChildren(long id)
        {
            var currentEvent = Get(id);
            RemoveEventAndHisChildren(currentEvent);
        }
    }
}