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

        public FrontBook(Book book, bool forWriter = false, User user = null) {
            Id = book.Id;
            Name = book.Name;
            Desc = book.Desc;
            RootChapterId = book.RootChapter != null ? book.RootChapter.Id : -1;
            OwnerId = book.Owner.Id;
            IsPublished = book.IsPublished;
            NumberOfChapters = book.AllChapters?.Sum(x => x.Desc.Length) ?? 0; //book.NumberOfChapters;
            NumberOfWords = book.AllChapters?.Sum(x => x.NumberOfWords) ?? 0; //book.NumberOfWords;
            Views = book.Travels?.Count() ?? 0;

            if (book.PublicationDate.HasValue) {
                PublicationDate = book.PublicationDate.Value.ToShortDateString();
            }

            AuthorFullName = book.Owner.Name;
            AuthorAvatar = book.Owner.AvatarUrl;

            if (forWriter) {
                ContainsCycle = new GraphHelper(book).HasCycle();
                IsReaded = false;
            } else {
                ContainsCycle = true;
                IsReaded = user == null
                    ? false
                    : book.Travels.Any(x => x.Reader.Id == user.Id);
            }

            //Genre = book.Genre != null ? new FrontGenre(book.Genre) : null;
            //AllEvents = book.AllChapters?.Select(x => new FrontChapter(x)).ToList();
            //Evaluations = book.Evaluations?.Select(x => new FrontEvaluation(x)).ToList();
            Tags = book.Tags?.Select(x => new FrontTag(x)).ToList();
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public long OwnerId { get; set; }
        public long RootChapterId { get; set; }
        public bool IsPublished { get; set; }
        public long NumberOfChapters { get; set; }
        public long NumberOfWords { get; set; }
        public string PublicationDate { get; set; }

        public long Views { get; set; }

        public bool IsReaded { get; set; }

        public string AuthorFullName { get; set; }
        public string AuthorAvatar { get; set; }

        public bool ContainsCycle { get; set; }
        //public FrontGenre Genre { get; set; }
        //public List<FrontChapter> AllEvents { get; set; }
        //public List<FrontEvaluation> Evaluations { get; set; }

        public List<FrontTag> Tags { get; set; }

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