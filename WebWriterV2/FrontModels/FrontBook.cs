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
            RootEventId = book.RootChapter != null ? book.RootChapter.Id : -1;
            OwnerId = book.Owner.Id;
            IsPublished = book.IsPublished;
            NumberOfChapters = book.NumberOfChapters;
            NumberOfWords = book.NumberOfWords;
            Views = book.Views;

            AuthorFullName = book.Owner.Name;
            AuthorAvatar = book.Owner.AvatarUrl;

            ContainsCycle = forWriter
                ? new GraphHelper(book).HasCycle()
                : true;

            //Genre = book.Genre != null ? new FrontGenre(book.Genre) : null;
            //AllEvents = book.AllChapters?.Select(x => new FrontChapter(x)).ToList();
            //Evaluations = book.Evaluations?.Select(x => new FrontEvaluation(x)).ToList();
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public long OwnerId { get; set; }
        public long RootEventId { get; set; }
        public bool IsPublished { get; set; }
        public long NumberOfChapters { get; set; }
        public long NumberOfWords { get; set; }

        public long Views { get; set; }

        public string AuthorFullName { get; set; }
        public string AuthorAvatar { get; set; }

        public bool ContainsCycle { get; set; }
        //public FrontGenre Genre { get; set; }
        //public List<FrontChapter> AllEvents { get; set; }
        //public List<FrontEvaluation> Evaluations { get; set; }

        public override Book ToDbModel() {
            return new Book {
                Id = Id,
                Name = Name,
                Desc = Desc,
                Owner = new User { Id = OwnerId },
                //RootChapter = RootEvent?.ToDbModel(),
                //AllChapters = AllEvents?.Select(x => x.ToDbModel()).ToList(),
                //Genre = Genre != null ? Genre.ToDbModel() : null,
                IsPublished = IsPublished
            };
        }
    }
}