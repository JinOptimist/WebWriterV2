using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dao.Model;
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
                for (int i = 0; i < 1000; i++) {
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
                return;
            }

            RandStep(travel, links[random.Next(links.Count)]);
        }

        public List<FrontChapter> StatisticOfVisitingAllWay()
        {
            Travels = new List<Travel>();
            foreach (var link in Book.RootChapter.LinksFromThisChapter) {
                var travel = CreateTravel();
                Step(travel, link);
            }

            var result = new List<FrontChapter>();

            var allTravelsCount = Travels.Count;
            foreach (var chapter in Book.AllChapters) {
                var countOfTravelBeHere = Travels.Count(tr => tr.Steps.Any(step => step.Choice.To.Id == chapter.Id));
                var percentOfUserBeHere = countOfTravelBeHere * 100 / allTravelsCount;

                var frontChapter = new FrontChapter(chapter);
                frontChapter.StatisticOfVisiting = Book.RootChapter.Id == chapter.Id ? 100 : percentOfUserBeHere;
                result.Add(frontChapter);
            }

            return result;
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
            var links =  travel.FilterLink(currentChapter.LinksFromThisChapter);

            if (!links.Any()) {
                return;
            }

            //first link use already exist travel
            Step(travel, links[0]);
            //all following links create new travel
            for (var i = 1; i < links.Count; i++) {
                var link = links[i];
                var newTravel = CopyTravel(travel);
                Step(newTravel, link);
            }
        }

        private Travel CreateTravel()
        {
            var travel = new Travel {
                Book = Book,
                State = new List<StateValue>(),
                Steps = new List<TravelStep>()
            };
            Travels.Add(travel);
            return travel;
        }

        private Travel CopyTravel(Travel originTravel)
        {
            var travel = CreateTravel();
            foreach(var originStep in originTravel.Steps) {
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