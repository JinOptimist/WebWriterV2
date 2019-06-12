using System;
using System.Collections.Generic;
using System.Linq;
using Dal.Model;
using NLog;
using WebWriterV2.FrontModel.BookMap;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class BookWithChaptersV2 : BaseFront<Book>
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private List<Tuple<long, long>> _mapChapterBranchIds = new List<Tuple<long, long>>();
        private List<Branch> _branches = new List<Branch>();

        public BookWithChaptersV2() { }

        public BookWithChaptersV2(Book book)
        {
            logger.Info("Start constructor BookWithChaptersV2");

            Id = book.Id;
            Name = book.Name;
            States = book.States.Select(x => new FrontStateType(x)).ToList();
            ContainsCycle = new GraphHelper(book).HasCycle();

            logger.Info($"Book contains cycle = {ContainsCycle}");

            book.AllChapters.ForEach(x => x.Level = 0);
            logger.Info($"Before maxDepth");
            var maxDepth = SetDepth(book);

            logger.Info($"maxDepth = {maxDepth}");

            //TODO: It's work to slow. Sad
            var elChel = new ElChel(book);
            //var elChapters = elChel.StatisticOfVisitingAllWay();
            logger.Info($"Before elChel.StatisticOfVisitingRandom. Time {DateTime.Now.ToLongTimeString()}");
            var elChapters = elChel.StatisticOfVisitingRandom(100);
            logger.Info($"After elChel.StatisticOfVisitingRandom. Time {DateTime.Now.ToLongTimeString()}");

            Chapters = new List<FrontChapter>();
            foreach (var chapter in book.AllChapters.Where(x => x.Level > 0).OrderBy(x => x.Level)) {
                var frontChapter = new FrontChapter(chapter);
                if (AvailableChild(chapter).Count > 1) {
                    var branch = CreateBranch(chapter);
                    frontChapter.Weight = branch.Width;
                }

                frontChapter.ParentsIds = GetParentIds(chapter, new List<long>());
                //frontChapter.StatisticOfVisiting = elChapters.Single(x => x.Id == frontChapter.Id).StatisticOfVisiting;
                Chapters.Add(frontChapter);
            }

            UpdateChapterWeightFromEnd();
            SetVisualParent();

            StateBasicTypes = EnumHelper.GetFrontEnumList<FrontEnumStateBasicType>(typeof(StateBasicType));
            CoAuthors = book.CoAuthors.Select(x => x.Email).ToList();
        }

        public string Name { get; set; }

        public bool ContainsCycle { get; set; }

        public List<FrontChapter> Chapters { get; set; }

        public List<FrontStateType> States { get; set; }
        public List<FrontEnum> StateBasicTypes { get; set; }

        public List<string> CoAuthors { get; set; }

        private int SetDepth(Book book)
        {
            var maxDepth = 1;
            var chaptersOnCurrentLevel = book.RootChapter.LinksFromThisChapter.Select(x => x.To);
            var visited = new List<Chapter> { book.RootChapter };
            book.RootChapter.Level = 1;
            
            while (chaptersOnCurrentLevel.Any()) {
                maxDepth++;
                var chaptersOnNextLevel = new List<Chapter>();
                foreach (var chapter in chaptersOnCurrentLevel) {
                    visited.Add(chapter);
                    chapter.Level = maxDepth;
                    chaptersOnNextLevel.AddRange(chapter.LinksFromThisChapter.Select(x => x.To));
                }

                chaptersOnCurrentLevel = chaptersOnNextLevel.Distinct();
                if (ContainsCycle) {
                    chaptersOnCurrentLevel = chaptersOnCurrentLevel.Where(x => !visited.Any(v => x.Id == v.Id));
                }
            }
            logger.Info($"SetDepth end. maxDepth = {maxDepth}. chaptersOnCurrentLevel.count == {chaptersOnCurrentLevel.Count()}. visited.count == {visited.Count()}");

            return maxDepth;
        }

        private Branch CreateBranch(Chapter rootChapter)
        {
            var branch = new Branch() {
                Id = rootChapter.Id,
                RootChapter = rootChapter,
                Chapters = new List<Chapter>() { rootChapter },
            };
            branch.Width = CountOfLinkOneLevelDown(rootChapter);

            var currentDepth = rootChapter.Level + 1;
            var childChapters = AvailableChild(rootChapter)
                .Where(x => x.Level == currentDepth);

            var currentDepthChapters = childChapters.Where(x => x.Level == currentDepth).ToList();
            var notProcessedChapters = childChapters.Where(x => x.Level != currentDepth).ToList();

            branch = RecursiveProcessedBranch(branch, currentDepthChapters, currentDepth, notProcessedChapters, branch.Width);
            _branches.Add(branch);
            return branch;
        }

        private Branch RecursiveProcessedBranch(Branch branch, List<Chapter> currentDepthChapters, int currentDepth, List<Chapter> notProcessedChapters, int currentWidth)
        {
            //currentDepthChapters.ForEach(x => currentWidth += CountOfLinkOneLevelDown(x) - x.LinksToThisChapter.Count);
            currentDepthChapters.ForEach(x => currentWidth += CountOfLinkOneLevelDown(x) - CountOfLinkOneLevelUp(x, branch));
            branch.Chapters.AddRange(currentDepthChapters);
            if (currentWidth > branch.Width) {
                branch.Width = currentWidth;
            }
            if (currentWidth <= 1) {
                //close branch
                return branch;
            }

            currentDepth++;

            //var nextDepthChapters = currentDepthChapters.SelectMany(ch => ch.LinksFromThisChapter.Select(link => link.To)).Distinct();
            var nextDepthChapters = currentDepthChapters.SelectMany(
                    currentDepthCh => AvailableChild(currentDepthCh).Where(ch => ch.Level == currentDepth)
                ).Distinct();

            currentDepthChapters = nextDepthChapters.Where(x => x.Level == currentDepth).ToList();
            notProcessedChapters.AddRange(nextDepthChapters.Where(x => x.Level > currentDepth)); // to be remove
            var additionalChaptersFromPreviusLevels = notProcessedChapters.Where(x => x.Level == currentDepth).ToList();
            currentDepthChapters.AddRange(additionalChaptersFromPreviusLevels);
            additionalChaptersFromPreviusLevels.ForEach(x => notProcessedChapters.Remove(x));

            currentDepthChapters = currentDepthChapters.Distinct().ToList();
            notProcessedChapters = notProcessedChapters.Distinct().ToList();

            return RecursiveProcessedBranch(branch, currentDepthChapters, currentDepth, notProcessedChapters, currentWidth);
        }

        private List<Chapter> AvailableChild(Chapter chapter)
        {
            var availableChildren = new List<Chapter>();

            var children = chapter.LinksFromThisChapter.Select(x => x.To);
            var chaptersWasAtLeastInOneBranch = _branches.SelectMany(x => x.Chapters);

            foreach (var child in children) {
                if (!chaptersWasAtLeastInOneBranch.Contains(child)) {
                    availableChildren.Add(child);
                    continue;
                }

                var childWasInAnotherBranch = _branches
                    .Where(x => x.Chapters.Contains(child))
                    .Any(oneOfBranchWhereChildAlreadyWas => {
                        return !oneOfBranchWhereChildAlreadyWas.Chapters.Select(x => x.Id).Contains(chapter.Id);
                    });

                if (childWasInAnotherBranch) {
                    continue;
                }

                availableChildren.Add(child);
            }

            return availableChildren;
        }

        private int CountOfLinkOneLevelDown(Chapter chapter)
        {
            //.Where(x => !_processedChapterIds.Contains(x.Id));
            //_processedChapterIds.AddRange(childChapters.Select(x => x.Id));

            return AvailableChild(chapter).Where(ch => ch.Level == chapter.Level + 1).Count();
            //return chapter.LinksFromThisChapter.Where(l => l.To.Level > chapter.Level).Count();
        }

        private int CountOfLinkOneLevelUp(Chapter chapter, Branch branch)
        {
            //.Where(x => !_processedChapterIds.Contains(x.Id));
            //_processedChapterIds.AddRange(childChapters.Select(x => x.Id));

            return chapter.LinksToThisChapter.Select(x => x.From)
                .Where(ch => ch.Level == chapter.Level - 1 && branch.Chapters.Contains(ch)).Count();
        }

        private List<long> GetParentIds(Chapter chapter, List<long> result)
        {
            foreach (var link in chapter.LinksToThisChapter) {
                if (link.From == null) {
                    continue;
                }
                var fromId = link.From.Id;
                if (result.Any(x => x == fromId)) {
                    continue;
                }

                result.Add(fromId);
                GetParentIds(link.From, result);
            }
            return result;
        }

        private void UpdateChapterWeightFromEnd()
        {
            foreach (var chapter in Chapters.OrderByDescending(x => x.Level)) {
                //if chapter has only one child
                if (chapter.LinksFromThisChapter.Count == 1) {
                    var toId = chapter.LinksFromThisChapter.Single().ToId;
                    var mySingleChild = Chapters.Single(x => x.Id == toId);
                    //and chapters shild has only one paren (current chapter)
                    if (mySingleChild.LinksToThisChapter.Count == 1) {
                        // current chapter get weight from child
                        chapter.Weight = mySingleChild.Weight;
                    }
                }
            }
        }

        private void SetVisualParent()
        {
            foreach (var chapter in Chapters) {
                var allBrachesWichIncludeCurrentChapter = _branches
                    .Where(x => x.Chapters.Select(ch => ch.Id).Contains(chapter.Id))
                    .Where(x => x.RootChapter.Id != chapter.Id);

                //just value by default
                chapter.VisualParentIds = new List<long>();

                foreach (var parentId in chapter.LinksToThisChapter.Select(x => x.FromId)) {
                    if (allBrachesWichIncludeCurrentChapter.All(b => b.Chapters.Select(x => x.Id).Contains(parentId))) {
                        chapter.VisualParentIds.Add(parentId);
                    }
                }
            }
        }

        public override Book ToDbModel()
        {
            throw new NotImplementedException();
        }
    }
}