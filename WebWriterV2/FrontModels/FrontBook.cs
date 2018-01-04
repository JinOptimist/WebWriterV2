using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class FrontBook : BaseFront<Book>
    {
        public FrontBook() {
        }

        public FrontBook(Book book, bool forWriter = false) {
            Id = book.Id;
            Name = book.Name;
            Desc = book.Desc;
            //RootEvent = book.RootChapter != null ? new FrontChapter(book.RootChapter) : null;
            //Genre = book.Genre != null ? new FrontGenre(book.Genre) : null;
            //AllEvents = book.AllChapters?.Select(x => new FrontChapter(x)).ToList();
            OwnerId = book.Owner.Id;
            //Evaluations = book.Evaluations?.Select(x => new FrontEvaluation(x)).ToList();
            IsPublished = book.IsPublished;
            NumberOfChapters = book.NumberOfChapters;
            NumberOfWords = book.NumberOfWords;

            ContainsCycle = forWriter
                ? new GraphHelper(book).HasCycle()
                : true;
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public long OwnerId { get; set; }
        //public FrontChapter RootEvent { get; set; }
        //public FrontGenre Genre { get; set; }
        //public List<FrontChapter> AllEvents { get; set; }
        //public List<FrontEvaluation> Evaluations { get; set; }
        public bool IsPublished { get; set; }
        public long NumberOfChapters { get; set; }
        public long NumberOfWords { get; set; }

        public bool ContainsCycle { get; set; }

        public override Book ToDbModel() {
            return new Book {
                Id = Id,
                Name = Name,
                Desc = Desc,
                //RootChapter = RootEvent?.ToDbModel(),
                //AllChapters = AllEvents?.Select(x => x.ToDbModel()).ToList(),
                Owner = new User { Id = OwnerId },
                //Genre = Genre != null ? Genre.ToDbModel() : null,
                IsPublished = IsPublished
            };
        }
    }
}