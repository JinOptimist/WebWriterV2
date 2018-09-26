using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;
using WebWriterV2.FrontModel.BookMap;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class BookWithChaptersV2 : BaseFront<Book>
    {
        public BookWithChaptersV2()
        {
        }

        public BookWithChaptersV2(Book book)
        {
            Id = book.Id;
            Name = book.Name;
            States = book.States.Select(x => new FrontStateType(x)).ToList();
            ContainsCycle = new GraphHelper(book).HasCycle();

            book.AllChapters.ForEach(x => x.Level = 0);
            var maxDepth = SetDepth(book.RootChapter, 1, new List<Chapter>());

            var elChel = new ElChel(book);
            //var elChapters = elChel.StatisticOfVisitingAllWay();
            var elChapters = elChel.StatisticOfVisiting100Random();

            Chapters = new List<FrontChapter>();
            foreach (var chapter in book.AllChapters.Where(x => x.Level > 0)) {
                var frontChapter = new FrontChapter(chapter);
                if (chapter.LinksFromThisChapter.Count > 1) {
                    var branch = CreateBranch(chapter);
                    frontChapter.Weight = branch.Width;
                }

                frontChapter.ParentsIds = GetParentIds(chapter, new List<long>());
                frontChapter.StatisticOfVisiting = elChapters.Single(x => x.Id == frontChapter.Id).StatisticOfVisiting;
                Chapters.Add(frontChapter);
            }

            StateBasicTypes = EnumHelper.GetFrontEnumList<FrontEnumStateBasicType>(typeof(StateBasicType));
        }

        public string Name { get; set; }

        public bool ContainsCycle { get; set; }

        public List<FrontChapter> Chapters { get; set; }

        public List<FrontBranch> Branches { get; set; }

        public List<FrontStateType> States { get; set; }
        public List<FrontEnum> StateBasicTypes { get; set; }



        private int SetDepth(Chapter chapter, int depth, List<Chapter> visitedChapters)
        {
            visitedChapters.Add(chapter);
            var maxDepth = depth;
            if (chapter.Level < depth) {
                chapter.Level = depth;
            }

            chapter.LinksFromThisChapter.ForEach(x => {
                if (visitedChapters.Any(v => v.Id == x.To.Id)) {
                    return;
                }

                var path = new List<Chapter>();
                path.AddRange(visitedChapters);

                var childDepth = SetDepth(x.To, chapter.Level + 1, path);
                if (childDepth > maxDepth) {
                    maxDepth = childDepth;
                }
            });

            return maxDepth;
        }

        private Branch CreateBranch(Chapter rootChapter)
        {
            var branch = new Branch() {
                RootChapter = rootChapter,
                Chapters = new List<Chapter>(),
                Width = LinkToDown(rootChapter).Count
            };
            var currentDepth = rootChapter.Level + 1;
            var childChapters = rootChapter.LinksFromThisChapter.Select(x => x.To);
            var currentDepthChapters = childChapters.Where(x => x.Level == currentDepth).ToList();
            var notProcessedChapters = childChapters.Where(x => x.Level != currentDepth).ToList();

            return RecursiveProcessedBranch(branch, currentDepthChapters, currentDepth, notProcessedChapters, branch.Width);
        }

        private Branch RecursiveProcessedBranch(Branch branch, List<Chapter> currentDepthChapters, int currentDepth, List<Chapter> notProcessedChapters, int currentWidth)
        {
            currentDepthChapters.ForEach(x => currentWidth += LinkToDown(x).Count - x.LinksToThisChapter.Count);
            branch.Chapters.AddRange(currentDepthChapters);
            if (currentWidth > branch.Width) {
                branch.Width = currentWidth;
            }
            if (currentWidth <= 1) {
                //close branch
                return branch;
            }

            currentDepth++;

            var nextDepthChapters = currentDepthChapters.SelectMany(ch => ch.LinksFromThisChapter.Select(link => link.To)).Distinct();
            currentDepthChapters = nextDepthChapters.Where(x => x.Level == currentDepth).ToList();
            notProcessedChapters.AddRange(nextDepthChapters.Where(x => x.Level > currentDepth));
            var additionalChaptersFromPreviusLevels = notProcessedChapters.Where(x => x.Level == currentDepth).ToList();
            currentDepthChapters.AddRange(additionalChaptersFromPreviusLevels);
            additionalChaptersFromPreviusLevels.ForEach(x => notProcessedChapters.Remove(x));

            currentDepthChapters = currentDepthChapters.Distinct().ToList();
            notProcessedChapters = notProcessedChapters.Distinct().ToList();

            return RecursiveProcessedBranch(branch, currentDepthChapters, currentDepth, notProcessedChapters, currentWidth);
        }

        private List<ChapterLinkItem> LinkToDown(Chapter chapter)
        {
            return chapter.LinksFromThisChapter.Where(l => l.To.Level > chapter.Level).ToList();
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

        public override Book ToDbModel()
        {
            throw new NotImplementedException();
        }
    }
}