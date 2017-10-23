using System.Data.Entity;
using Dao.IRepository;
using Dao.Model;
using System;
using System.Linq;

namespace Dao.Repository
{
    public class ChapterLinkItemRepository : BaseRepository<ChapterLinkItem>, IChapterLinkItemRepository
    {
        private readonly Lazy<ChapterRepository> _eventRepository;
        private readonly Lazy<StateRepository> _stateRepository;
        private readonly Lazy<ThingRepository> _thingRepository;

        public ChapterLinkItemRepository(WriterContext db) : base(db)
        {
            _eventRepository = new Lazy<ChapterRepository>(() => new ChapterRepository(db));
            _stateRepository = new Lazy<StateRepository>(() => new StateRepository(db));
            _thingRepository = new Lazy<ThingRepository>(() => new ThingRepository(db));
        }

        public override ChapterLinkItem Save(ChapterLinkItem model)
        {
            if (_eventRepository.Value.Db.Entry(model.From).State == EntityState.Detached)
            {
                model.From = _eventRepository.Value.Entity.Find(model.From.Id);
            }

            if (_eventRepository.Value.Db.Entry(model.To).State == EntityState.Detached)
            {
                model.To = _eventRepository.Value.Entity.Find(model.To.Id);
            }

            return base.Save(model);
        }

        public void RemoveDuplicates()
        {
            var duplicate = Entity.Where(x => true)
                .GroupBy(x => new {x.From, x.Text, x.To})
                .SelectMany(x => x.OrderBy(y => y.Id).Skip(1));
            Remove(duplicate);
        }

        public override void Remove(ChapterLinkItem baseModel)
        {
            if (baseModel.ThingsChanges?.Any() ?? false) {
                _thingRepository.Value.Remove(baseModel.ThingsChanges);
            }
            if (baseModel.RequirementThings?.Any() ?? false) {
                _thingRepository.Value.Remove(baseModel.RequirementThings);
            }
            if (baseModel.RequirementStates?.Any() ?? false) {
                _stateRepository.Value.Remove(baseModel.RequirementStates);
            }
            if (baseModel.HeroStatesChanging?.Any() ?? false) {
                _stateRepository.Value.Remove(baseModel.HeroStatesChanging);
            }

            base.Remove(baseModel);
        }
    }
}