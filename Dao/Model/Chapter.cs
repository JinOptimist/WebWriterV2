using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dao.Model
{
    public class Chapter : BaseModel, IUpdatable<Chapter>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Desc { get; set; }

        public long NumberOfWords { get; set; }

        /// <summary>
        /// You must save this field separately. Use EventLinkItemRepository
        /// </summary>
        public virtual List<ChapterLinkItem> LinksFromThisChapter { get; set; }

        /// <summary>
        /// You must save this field separately. Use EventLinkItemRepository
        /// </summary>
        public virtual List<ChapterLinkItem> LinksToThisChapter { get; set; }

        //public virtual Location RequrmentLocation { get; set; }
        public virtual Book Book { get; set; }
        public virtual Book ForRootBook { get; set; }

        public void UpdateFrom(Chapter model)
        {
            if (Id != model.Id)
                throw new Exception($"You try update Event with Id: {model.Id} from Event with id {Id}");
            Name = model.Name;
            Desc = model.Desc;
            Book = model.Book;
            ForRootBook = model.ForRootBook;
            NumberOfWords = model.NumberOfWords;
        }
    }
}