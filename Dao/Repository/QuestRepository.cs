using System;
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
        private readonly EventLinkItemRepository _eventLinkItemRepository;
        private readonly EvaluationRepository _evaluationRepository;

        public QuestRepository(WriterContext db) : base(db)
        {
            _eventRepository = new EventRepository(db);
            _eventLinkItemRepository = new EventLinkItemRepository(db);
            _evaluationRepository = new EvaluationRepository(db);
        }

        public List<Quest> GetAllWithRootEvent()
        {
            return Entity.Include(x => x.RootEvent).ToList();
        }

        public List<Quest> GetByUser(long userId)
        {
            return Entity.Where(x => x.Owner.Id == userId).ToList();
        }

        public override void Remove(Quest quest)
        {
            if (quest == null)
                return;

            _evaluationRepository.Remove(quest.Evaluations);

            foreach (var @event in quest.AllEvents)
            {
                _eventLinkItemRepository.Remove(@event.LinksFromThisEvent);
                _eventLinkItemRepository.Remove(@event.LinksToThisEvent);
            }

            if (quest.AllEvents.Count > 0)
            {
                _eventRepository.Remove(quest.AllEvents);
            }

            quest = Get(quest.Id);
            base.Remove(quest);
        }

        /// <summary>
        /// If we update model, we update only Name and Desc field. Don't forget this
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override Quest Save(Quest model)
        {
            // if we try update detached model
            if (Db.Entry(model).State == EntityState.Detached && model.Id > 0)
            {
                var modelFromDb = Get(model.Id);
                modelFromDb.UpdateFrom(model);
                model = modelFromDb;
            }

            return base.Save(model);
        }

        public Quest GetByName(string name)
        {
            return Entity.FirstOrDefault(x => x.Name == name);
        }
    }
}