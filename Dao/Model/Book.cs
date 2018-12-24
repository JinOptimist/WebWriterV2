using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dal.Model
{
    public class Book : BaseModel, IUpdatable<Book>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Desc { get; set; }

        [Description("Начальная глава")]
        public virtual Chapter RootChapter { get; set; }

        public virtual User Owner { get; set; }
        public virtual List<User> CoAuthors { get; set; }
        public virtual List<StateType> States { get; set; }
        public virtual List<Chapter> AllChapters { get; set; }
        public virtual List<Evaluation> Evaluations { get; set; }
        public virtual List<UserWhoReadBook> Readers { get; set; }
        public virtual List<Travel> Travels { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual List<Tag> Tags { get; set; }

        public bool IsPublished { get; set; }
        public virtual DateTime? PublicationDate { get; set; }
        public int Views { get; set; }

        public long NumberOfChapters { get; set; }
        public long NumberOfWords { get; set; }

        public void UpdateFrom(Book model)
        {
            if (Id != model.Id) {
                throw new Exception($"You try update Book model with id {Id} from model with id {Id}");
            }

            Name = model.Name;
            Desc = model.Desc;
            NumberOfChapters = model.NumberOfChapters;
            NumberOfWords = model.NumberOfWords;

            //TODO Update collection element by element
            //RootEvent = model.RootEvent;
            //Tips = model.Tips;
            //AllEvents = model.AllEvents;
        }
    }
}

