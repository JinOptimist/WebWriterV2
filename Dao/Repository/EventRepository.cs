using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(WriterContext db) : base(db)
        {
        }

        /// <summary>
        /// Special realization for cascade deleting
        /// </summary>
        /// <param name="currentEvent"></param>
        public override void Remove(Event currentEvent)
        {
            if (currentEvent == null)
                return;
            currentEvent.ChildrenEvents = currentEvent.ChildrenEvents ?? new List<Event>();

            if (currentEvent.ChildrenEvents.Any())
            {
                var forDelete = currentEvent.ChildrenEvents.ToList();
                forDelete.ForEach(Remove);
            }

            var isDetached = Db.Entry(currentEvent).State == EntityState.Detached;
            if (isDetached)
            {
                return;
            }

            currentEvent.ChildrenEvents = null;
            Save(currentEvent);

            var pe = currentEvent.ParentEvents.ToList();
            foreach (var someEvent in pe)
            {
                someEvent.ChildrenEvents.Remove(currentEvent);
                Save(someEvent);
            }

            base.Remove(currentEvent);
        }

        public List<Event> GetByQuest(long questId)
        {
            return Entity.Where(x => x.Quest.Id == questId).ToList();
        }

        public List<Event> GetRootEvents()
        {
            return Entity.Where(x => x.Quest != null).ToList();
        }
    }
}