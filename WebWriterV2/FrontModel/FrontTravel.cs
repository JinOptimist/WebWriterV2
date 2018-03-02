using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontTravel : BaseFront<Travel>
    {
        public FrontTravel()
        {
        }

        public FrontTravel(Travel travel)
        {
            Id = travel.Id;
            if (travel.Book != null) {
                Book = new FrontBook(travel.Book);
            }
            if (travel.CurrentChapter != null) {
                Chapter = new FrontChapter(travel.CurrentChapter, travel);
            }

            AllStates = string.Join(", ", travel.State);
        }

        public FrontBook Book { get; set; }
        public FrontChapter Chapter { get; set; }
        public string AllStates { get; set; }

        public override Travel ToDbModel()
        {
            throw new NotImplementedException();
        }
    }
}
