using Dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xceed.Words.NET;

namespace WebWriterV2.RpgUtility
{
    public class DocXHelper
    {
        private DocX doc;
        private string path;
        private Travel travel;
        private Formatting chapterTitleFormatting;
        private Formatting choiceFormatting;
        private Formatting cancelChoiceFormatting;
        public DocXHelper(Travel travel)
        {
            this.travel = travel;

            chapterTitleFormatting = new Formatting();
            chapterTitleFormatting.Size = 18;
            chapterTitleFormatting.Bold = true;

            cancelChoiceFormatting = new Formatting();
            cancelChoiceFormatting.StrikeThrough = StrikeThrough.strike;
            cancelChoiceFormatting.Italic = true;

            choiceFormatting = new Formatting();
            choiceFormatting.UnderlineStyle = UnderlineStyle.dash;
            choiceFormatting.Italic = true;

            path = PathHelper.PathToBook(travel.Id);

            if (System.IO.File.Exists(path)) {
                System.IO.File.Delete(path);
            }
            doc = DocX.Create(path);
        }

        public string GenerateDocx()
        {
            doc.InsertParagraph(travel.Book.RootChapter.Name, false, chapterTitleFormatting);
            doc.InsertParagraph(travel.Book.RootChapter.Desc);

            var rootStep = travel.Steps.First(x => x.PrevStep == null);
            WriteStep(rootStep.NextStep);

            //travel.IsTravelEnd

            doc.Save();

            return path;
        }

        public void WriteStep(TravelStep step)
        {
            var links = step.PrevStep.CurrentChapter.LinksFromThisChapter;
            var choice = step.Choice;
            foreach (var link in links) {
                var linkText = string.IsNullOrWhiteSpace(link.Text) ? "Далее" : link.Text;
                if (choice.Id == link.Id) {
                    doc.InsertParagraph(linkText, false, choiceFormatting);
                } else {
                    doc.InsertParagraph(linkText, false, cancelChoiceFormatting);
                }
            }

            doc.InsertParagraph();
            doc.InsertParagraph(step.CurrentChapter.Name, false, chapterTitleFormatting);
            doc.InsertParagraph(step.CurrentChapter.Desc);

            if (step.NextStep != null)
                WriteStep(step.NextStep);
        }
    }
}