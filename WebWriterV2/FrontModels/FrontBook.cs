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
            RootEvent = book.RootEvent != null ? new FrontEvent(book.RootEvent) : null;
            Genre = book.Genre != null ? new FrontGenre(book.Genre) : null;
            AllEvents = book.AllEvents?.Select(x => new FrontEvent(x)).ToList();
            OwnerId = book.Owner?.Id;
            Evaluations = book.Evaluations?.Select(x => new FrontEvaluation(x)).ToList();
            IsPublished = book.IsPublished;
            NumberOfChapters = book.NumberOfChapters;
            NumberOfWords = book.NumberOfWords;

            ContainsCycle = forWriter
                ? new GraphHelper(book).HasCycle()
                : true;
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public long? OwnerId { get; set; }
        public FrontEvent RootEvent { get; set; }
        public FrontGenre Genre { get; set; }
        public List<FrontEvent> AllEvents { get; set; }
        public List<FrontEvaluation> Evaluations { get; set; }
        public bool IsPublished { get; set; }
        public long NumberOfChapters { get; set; }
        public long NumberOfWords { get; set; }

        public bool ContainsCycle { get; set; }

        public override Book ToDbModel() {
            return new Book {
                Id = Id,
                Name = Name,
                Desc = Desc,
                RootEvent = RootEvent?.ToDbModel(),
                AllEvents = AllEvents?.Select(x => x.ToDbModel()).ToList(),
                Owner = OwnerId.HasValue ? new User { Id = OwnerId.Value } : null,
                Genre = Genre != null ? Genre.ToDbModel() : null,
                IsPublished = IsPublished
            };
        }
    }
}