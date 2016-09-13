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
            if (currentEvent.ChildrenEvents == null)
                currentEvent = Get(currentEvent.Id);

            if (!currentEvent.ChildrenEvents.Any())
            {
                base.Remove(currentEvent);
                return;
            }

            var forDelete = currentEvent.ChildrenEvents.ToList();
            forDelete.ForEach(Remove);
            base.Remove(currentEvent);
        }

        public override void Remove(long id)
        {
            var currentEvent = Get(id);
            Remove(currentEvent);
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