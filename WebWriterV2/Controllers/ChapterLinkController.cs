using Dao;
using Dao.IRepository;
using Dao.Model;
using Dao.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using WebWriterV2.DI;
using WebWriterV2.FrontModels;
using WebWriterV2.VkUtility;

namespace WebWriterV2.Controllers
{
    public class ChapterLinkController : BaseApiController
    {
        public ChapterLinkController(IBookRepository bookRepository, IChapterLinkItemRepository chapterLinkItemRepository, IStateTypeRepository stateTypeRepository, IChapterRepository chapterRepository, IStateChangeRepository stateChangeRepository, IStateRequirementRepository stateRequirementRepository)
        {
            BookRepository = bookRepository;
            ChapterLinkItemRepository = chapterLinkItemRepository;
            StateTypeRepository = stateTypeRepository;
            ChapterRepository = chapterRepository;
            StateChangeRepository = stateChangeRepository;
            StateRequirementRepository = stateRequirementRepository;
        }

        private IBookRepository BookRepository { get; set; }
        private IChapterLinkItemRepository ChapterLinkItemRepository { get; set; }
        private IStateTypeRepository StateTypeRepository { get; set; }
        private IChapterRepository ChapterRepository { get; set; }
        private IStateChangeRepository StateChangeRepository { get; set; }
        private IStateRequirementRepository StateRequirementRepository { get; set; }

        [AcceptVerbs("POST")]
        public FrontChapterLinkItem Save(FrontChapterLinkItem frontChapterLinkItem)
        {
            var сhapterLinkItem = frontChapterLinkItem.ToDbModel();
            сhapterLinkItem = ChapterLinkItemRepository.Save(сhapterLinkItem);
            frontChapterLinkItem = new FrontChapterLinkItem(сhapterLinkItem);
            return frontChapterLinkItem;
        }

        [AcceptVerbs("GET")]
        public List<FrontChapterLinkItem> GetLinksFromChapter(long chapterId)
        {
            var linksFromChapter = ChapterLinkItemRepository.GetLinksFromChapter(chapterId);
            return linksFromChapter.Select(x => new FrontChapterLinkItem(x)).ToList();
        }

        [AcceptVerbs("GET")]
        public bool Remove(long id)
        {
            ChapterLinkItemRepository.Remove(id);
            return true;
        }

        [AcceptVerbs("GET")]
        public bool LinkDecisionToChapterLink(long chapterId, string decision)
        {
            var chapterLink = ChapterLinkItemRepository.Get(chapterId);
            if (chapterLink.StateChanging == null) {
                chapterLink.StateChanging = new List<StateChange>();
            }

            var oldValue = chapterLink.StateChanging.SingleOrDefault()?.Text;
            if (oldValue != decision) {
                StateChangeRepository.RemoveDecision(oldValue, chapterLink.From.Book.Id);
                StateRequirementRepository.RemoveDecision(oldValue, chapterLink.From.Book.Id);
            }

            var defaultStateType = StateTypeRepository.GetDefault();
            chapterLink.StateChanging.Add(new StateChange() {
                ChapterLink = chapterLink,
                Text = decision,
                ChangeType = ChangeType.Create,
                StateType = defaultStateType
            });

            ChapterLinkItemRepository.Save(chapterLink);

            return true;
        }

        [AcceptVerbs("GET")]
        public bool LinkConditionToChapterLink(long chapterId, string condition)
        {
            var chapterLink = ChapterLinkItemRepository.Get(chapterId);
            if (chapterLink.StateRequirement == null) {
                chapterLink.StateRequirement = new List<StateRequirement>();
            }

            var oldValue = chapterLink.StateRequirement.SingleOrDefault()?.Text;
            if (oldValue != condition) {
                StateRequirementRepository.RemoveDecision(oldValue, chapterLink.From.Book.Id);
            }

            var defaultStateType = StateTypeRepository.GetDefault();
            chapterLink.StateRequirement.Add(new StateRequirement() {
                ChapterLink = chapterLink,
                Text = condition,
                StateType = defaultStateType,
                RequirementType = RequirementType.Exist,
            });
            ChapterLinkItemRepository.Save(chapterLink);

            return true;
        }

        [AcceptVerbs("GET")]
        public List<string> GetAvailableDecision(long chapterId)
        {
            var chapter = ChapterRepository.Get(chapterId);
            var allLinks = chapter.Book.AllChapters.SelectMany(x => x.LinksToThisChapter);
            var stateChanges = allLinks.SelectMany(x => x.StateChanging);
            var solutions = stateChanges.Select(x => x.Text);
            return solutions.ToList();
        }
    }
}
