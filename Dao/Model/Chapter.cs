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
        /// Number of chapter level in book
        /// Root chapter level is 1
        /// </summary>
        public int Level { get; set; }

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
                throw new Exception($"You try update one Chapter from another. Chapter to Id: {model.Id} Chapter from id {Id}");
            Name = model.Name;
            Desc = model.Desc;
            Book = model.Book;
            ForRootBook = model.ForRootBook;
            NumberOfWords = model.NumberOfWords;
        }

        public void AddChildEvent(Chapter addedEvent, string text = null)
        {
            ConnecteEvent(this, addedEvent, text);
        }

        public void AddParentEvent(Chapter addedEvent, string text = null)
        {
            ConnecteEvent(addedEvent, this, text);
        }

        public void AddChildrenEvents(params Chapter[] childrenEvents)
        {
            foreach (var childEvent in childrenEvents) {
                ConnecteEvent(this, childEvent);
            }
        }

        public void AddParentsEvents(string text = null, params Chapter[] parentsEvents)
        {
            foreach (var parentEvent in parentsEvents) {
                ConnecteEvent(parentEvent, this, text);
            }
        }

        private void ConnecteEvent(Chapter parentEvent, Chapter childEvent, string text = null)
        {
            if (parentEvent.LinksFromThisChapter == null)
                parentEvent.LinksFromThisChapter = new List<ChapterLinkItem>();
            if (childEvent.LinksToThisChapter == null)
                childEvent.LinksToThisChapter = new List<ChapterLinkItem>();

            var eventLinkItem = new ChapterLinkItem {
                From = parentEvent,
                To = childEvent,
                Text = text ?? childEvent.Name
            };

            parentEvent.LinksFromThisChapter.Add(eventLinkItem);
            childEvent.LinksToThisChapter.Add(eventLinkItem);
        }
    }
}