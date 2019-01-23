using System.Data.Entity;
using Dal.IRepository;
using Dal.Model;
using System;
using System.Linq;
using Dal.Repository;
using System.Collections.Generic;

namespace Dal.Repository
{
    public class ChapterLinkItemRepository : BaseRepository<ChapterLinkItem>, IChapterLinkItemRepository
    {
        private readonly Lazy<ChapterRepository> _eventRepository;
        private readonly StateRequirementRepository _stateRequirementRepository;
        private readonly StateChangeRepository _stateChangeRepository;
        private readonly TravelStepRepository _travelStepRepository;

        public ChapterLinkItemRepository(WriterContext db) : base(db)
        {
            _eventRepository = new Lazy<ChapterRepository>(() => new ChapterRepository(db));
            _stateRequirementRepository = new StateRequirementRepository(db);
            _stateChangeRepository = new StateChangeRepository(db);
            _travelStepRepository = new TravelStepRepository(db);
        }

        public override ChapterLinkItem Save(ChapterLinkItem model)
        {
            model.From = model.From.AttachIfNot(Db);

            model.To = model.To.AttachIfNot(Db);

            return base.Save(model);
        }

        public void RemoveDuplicates()
        {
            var duplicate = Entity.Where(x => true)
                .GroupBy(x => new {x.From, x.Text, x.To})
                .SelectMany(x => x.OrderBy(y => y.Id).Skip(1));
            Remove(duplicate);
        }

        public List<ChapterLinkItem> GetLinksFromChapter(long chapterId)
        {
            return Entity.Where(x => x.From.Id == chapterId).ToList();
        }

        public override void Remove(ChapterLinkItem chapterLink)
        {
            _stateChangeRepository.Remove(chapterLink.StateChanging);
            _stateRequirementRepository.Remove(chapterLink.StateRequirement);

            var stepsToRemove = chapterLink.TravelSteps;
            foreach(var step in stepsToRemove) {
                var prev = step.PrevStep;
                var next = step.NextStep;
                if (prev != null) {
                    prev.NextStep = next;
                }
                if (next != null) {
                    next.PrevStep = prev;
                }
                if (step.Travel.CurrentStep == step && prev != null) {
                    step.Travel.CurrentStep = prev;
                }

                _travelStepRepository.Save(prev);
            }

            _travelStepRepository.Remove(stepsToRemove);

            base.Remove(chapterLink);
        }

        public bool Exist(long fromId, long toId)
        {
            return Entity.Any(x => x.From.Id == fromId && x.To.Id == toId);
        }
    }
}