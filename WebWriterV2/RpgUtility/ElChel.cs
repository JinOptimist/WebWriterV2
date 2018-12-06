using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dal.Model;
using WebWriterV2.FrontModels;
using System.Net.Mail;
using System.Net;

namespace WebWriterV2.RpgUtility
{
    public class ElChel
    {
        private Book Book { get; set; }
        private List<Travel> Travels { get; set; }
        private Random random = new Random();

        private int CountOfRandomElChel = 1000;

        public ElChel(Book book)
        {
            Book = book;
            Travels = new List<Travel>();
        }

        public List<FrontChapter> StatisticOfVisiting100Random()
        {
            Travels = new List<Travel>();

            var links = Book.RootChapter.LinksFromThisChapter;
            if (links.Any()) {
                for (int i = 0; i < CountOfRandomElChel; i++) {
                    var travel = CreateTravel();
                    RandStep(travel, links[random.Next(links.Count)]);
                }
            }

            var result = new List<FrontChapter>();

            var allTravelsCount = Travels.Count > 0 ? Travels.Count : 1;
            foreach (var chapter in Book.AllChapters) {
                var countOfTravelBeHere = Travels.Count(tr => tr.Steps.Any(step => step.Choice.To.Id == chapter.Id));
                var percentOfUserBeHere = countOfTravelBeHere * 100 / allTravelsCount;

                var frontChapter = new FrontChapter(chapter);
                frontChapter.StatisticOfVisiting = Book.RootChapter.Id == chapter.Id ? 100 : percentOfUserBeHere;
                result.Add(frontChapter);
            }

            return result;
        }

        private void RandStep(Travel travel, ChapterLinkItem linkItem)
        {
            var currentChapter = linkItem.To;
            // 1) ApplyChanges
            StateHelper.ApplyChangeToTravel(travel, linkItem.StateChanging);
            // ? 2) New step
            var step = new TravelStep {
                Choice = linkItem
            };
            travel.Steps.Add(step);

            // 3) Fillter Links
            var links = travel.FilterLink(currentChapter.LinksFromThisChapter);

            if (!links.Any() || travel.Steps.Count > 100) {
                Travels.Add(travel);
                return;
            }

            RandStep(travel, links[random.Next(links.Count)]);
        }

        public List<Travel> StatisticOfVisitingAllWay()
        {
            Travels = new List<Travel>();
            foreach (var link in Book.RootChapter.LinksFromThisChapter) {
                var travel = CreateTravel();
                Step(travel, link);
            }
            // we get all possible travels

            //var chapterLinkIds = Travels.SelectMany(x => x.Steps.Select(s => s.Choice.Id)).Distinct().ToList();
            //var steps = Travels.SelectMany(x => x.Steps);
            //var uniqChoices = new List<ChapterLinkItem>();
            //foreach(var step in steps) {
            //    if (chapterLinkIds.Contains(step.Choice.Id)) {
            //        uniqChoices.Add(step.Choice);
            //        chapterLinkIds.Remove(step.Choice.Id);
            //    }
            //}

            //foreach(var ch in uniqChoices) {
            //    var travelsWhichDidTheChoice = Travels.Where(x => x.Steps.Any(s => s.Choice.Id == ch.Id));
            //    var travelsWhichDidNotTheChoice = Travels.Where(x => !x.Steps.Any(s => s.Choice.Id == ch.Id));
            //}
            

            return Travels;
        }

        private int DiffChaptersBetweenListOfTravels(IEnumerable<Travel> travels1, IEnumerable<Travel> travels2)
        {
            var chaptes1 = travels1.SelectMany(x => x.Steps.Select(st => st.Choice.To));
            var chaptes2 = travels2.SelectMany(x => x.Steps.Select(st => st.Choice.To));

            //chaptes1.ToList().RemoveAll(x=>x.);

            return 0;
        }

        private void Step(Travel travel, ChapterLinkItem linkItem)
        {
            var currentChapter = linkItem.To;
            // 1) ApplyChanges
            StateHelper.ApplyChangeToTravel(travel, linkItem.StateChanging);
            // ? 2) New step
            var step = new TravelStep {
                Choice = linkItem
            };
            travel.Steps.Add(step);

            // 3) Fillter Links
            var alreadyVisitedLinks = travel.Steps.Select(st => st.Choice.Id);
            var currentChapterLinks = currentChapter.LinksFromThisChapter.Where(x => !alreadyVisitedLinks.Contains(x.Id));
            if (!currentChapterLinks.Any() && currentChapter.LinksFromThisChapter.Any()) {
                currentChapterLinks = currentChapter.LinksFromThisChapter;
            }
            var links = travel.FilterLink(currentChapterLinks);

            if (!links.Any()) {
                Travels.Add(travel);
                return;
            }

            //first link use already exist travel
            Travel travelDuplication = null;
            if (links.Count > 1) {
                travelDuplication = CopyTravel(travel);
            }
            Step(travel, links[0]);
            //all following links create new travel
            for (var i = 1; i < links.Count; i++) {
                var link = links[i];
                var newTravel = CopyTravel(travelDuplication);
                Step(newTravel, link);
            }
        }

        private Travel CreateTravel()
        {
            return new Travel {
                Book = Book,
                State = new List<StateValue>(),
                Steps = new List<TravelStep>()
            };
        }

        private Travel CopyTravel(Travel originTravel)
        {
            var travel = CreateTravel();
            foreach (var originStep in originTravel.Steps) {
                var step = new TravelStep {
                    Choice = originStep.Choice
                };
                travel.Steps.Add(originStep);
                StateHelper.ApplyChangeToTravel(travel, originStep.Choice.StateChanging);
            }

            return travel;
        }
    }
}