﻿using Dal;
using Dal.IRepository;
using Dal.Model;
using Dal.Repository;
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
        public FrontChapterLinkItem CreateLink(long fromId, long toId)
        {
            if (ChapterLinkItemRepository.Exist(fromId, toId)) {
                return null;
            }

            var fromChapter = new Chapter { Id = fromId };
            var toChapter = new Chapter { Id = toId };

            var link = new ChapterLinkItem() {
                From = fromChapter,
                To = toChapter,
                Text = string.Empty
            };

            link = ChapterLinkItemRepository.Save(link);
            return new FrontChapterLinkItem(link);
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
            var link = ChapterLinkItemRepository.Get(id);
            // If we try to remove link to chapter which has only the single link.
            if (link.To.LinksToThisChapter.Count == 1) {
                var linkToTheSameChapter = link.To.LinksToThisChapter.Single();
                // If we try remove link to root chapter it's normal
                // Another way, say No.
                if (linkToTheSameChapter.To.ForRootBook == null) {
                    return false;
                }
            }

            ChapterLinkItemRepository.Remove(link);
            return true;
        }


       

        [AcceptVerbs("POST")]
        public FrontStateChange AddStateChange(FrontStateChange frontStateChange)
        {
            var stateChange = frontStateChange.ToDbModel();
            StateChangeRepository.Save(stateChange);
            return new FrontStateChange(stateChange);
        }

        [AcceptVerbs("GET")]
        public bool RemoveStateChange(long stateChangeId)
        {
            StateChangeRepository.Remove(stateChangeId);
            return true;
        }

        [AcceptVerbs("POST")]
        public FrontStateRequirement AddStateRequirement(FrontStateRequirement frontStateRequirement)
        {
            var stateRequirement = frontStateRequirement.ToDbModel();
            StateRequirementRepository.Save(stateRequirement);
            return new FrontStateRequirement(stateRequirement);
        }

        [AcceptVerbs("GET")]
        public bool RemoveStateRequirement(long stateRequirementId)
        {
            StateRequirementRepository.Remove(stateRequirementId);
            return true;
        }




        // REMOVE IT
        [AcceptVerbs("GET")]
        public List<string> GetAvailableDecision(long chapterId)
        {
            var chapter = ChapterRepository.Get(chapterId);
            var allLinks = chapter.Book.AllChapters.SelectMany(x => x.LinksToThisChapter);
            var stateChanges = allLinks.SelectMany(x => x.StateChanging);
            var solutions = stateChanges.Select(x => x.Text);
            return solutions.ToList();
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
                //ChangeType = ChangeType.Create,
                ChangeType = ChangeType.Set,
                StateType = defaultStateType
            });

            ChapterLinkItemRepository.Save(chapterLink);

            return true;
        }


    }
}
