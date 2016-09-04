using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        /// <summary>
        /// Special realization for cascade deleting
        /// </summary>
        /// <param name="currentEvent"></param>
        public new void Remove(Event currentEvent)
        {
            if (currentEvent.ChildrenEvents == null)
                currentEvent = GetWithParentAndChildren(currentEvent.Id);

            if (!currentEvent.ChildrenEvents.Any())
            {
                base.Remove(currentEvent);
                return;
            }

            var forDelete = currentEvent.ChildrenEvents.ToList();
            forDelete.ForEach(eve => Remove(eve));
            base.Remove(currentEvent);
        }

        public new void Remove(long id)
        {
            var currentEvent = GetWithParentAndChildren(id);
            Remove(currentEvent);
        }

        public Event GetWithParentAndChildren(long id)
        {
            return Entity.Include(x => x.ChildrenEvents).Include(x => x.ParentEvents).FirstOrDefault(x => x.Id == id);
        }

        public List<Event> GetEvents(long questId)
        {
            return Entity.Where(x => x.Quest.Id == questId).ToList();
        }
    }
}