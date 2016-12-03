using System;
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

        public override Event Save(Event model)
        {
            var children = model.ChildrenEvents;
            var parents = model.ParentEvents;
            var find = Entity.Find(model.Id);
            if (find != null)
            {
                find.Update(model);
                model = find;
            }
            else
            {
                if (model.Id > 0)
                    Entity.Attach(model);
                else
                    Entity.Add(model);
            }

            if (model.ChildrenEvents != null)
            {
                foreach (var child in children.Where(x => model.ChildrenEvents.All(u => u.Id != x.Id)))
                {
                    if (Db.Entry(child).State != EntityState.Detached)
                    {
                        continue;
                    }
                    var childEvent = Entity.Find(child.Id);
                    if (childEvent == null)
                    {
                        childEvent = new Event { Id = child.Id };
                        Entity.Attach(childEvent);
                    }
                    model.ChildrenEvents.Add(childEvent);
                }

                var forRemove = model.ChildrenEvents.Where(x => children.All(u => u.Id != x.Id)).ToList();
                foreach (var child in forRemove)
                {
                    model.ChildrenEvents.Remove(child);
                }
            }


            return base.Save(model);
        }

        public override void Remove(Event currentEvent)
        {
            if (currentEvent == null)
                return;

            if (HasChild(currentEvent.Id))
            {
                throw new Exception("You can't remove event wich has child");
            }
            
            base.Remove(currentEvent);
        }

        public void RemoveEventAndChildren(Event currentEvent)
        {
            if (currentEvent == null)
                return;

            if (HasChild(currentEvent.Id))
            {
                throw new Exception("You can't remove event wich has child");
            }

            base.Remove(currentEvent);
        }

        /// <summary>
        /// Special realization for cascade deleting
        /// </summary>
        /// <param name="currentEvent"></param>
        public void RemoveWholeBranch(long currentEventId) {
            RemoveWholeBranch(Get(currentEventId));
        }

        /// <summary>
        /// Special realization for cascade deleting
        /// </summary>
        /// <param name="currentEvent"></param>
        public void RemoveWholeBranch(Event currentEvent)
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

        public List<Event> GetAllEventsByQuest(long questId)
        {
            return Entity.Where(x => x.Quest.Id == questId).ToList();
        }

        public List<Event> GetRootEvents(long questId)
        {
            return Entity.Where(x => x.Quest != null && x.Quest.Id == questId).ToList();
        }

        public bool HasChild(long eventId)
        {
            return Entity.Any(x => x.Id == eventId);
        }
    }
}