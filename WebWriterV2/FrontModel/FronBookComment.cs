using System;
using System.Collections.Generic;
using System.Linq;
using Dal.Model;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class FronBookComment : BaseFront<BookComment>
    {
        public FronBookComment()
        {
        }

        public FronBookComment(BookComment bookComment)
        {
            Id = bookComment.Id;
            Text = bookComment.Text;
            PublicationDate = $"{bookComment.PublicationDate.ToShortDateString()} {bookComment.PublicationDate.ToShortTimeString()}";

            AuthorId = bookComment.Author.Id;
            BookId = bookComment.Book.Id;
        }

        public string Text { get; set; }
        public string PublicationDate { get; set; }

        public long AuthorId { get; set; }
        public long BookId { get; set; }

        public override BookComment ToDbModel()
        {
            return new BookComment
            {
                Id = Id,
                Text = Text,
                Author = new User { Id = AuthorId },
                Book = new Book { Id = BookId }
            };
        }
    }
}