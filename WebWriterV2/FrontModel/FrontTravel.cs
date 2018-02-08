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
            Book = new FrontBook(travel.Book);
            if (travel.CurrentChapter != null) {
                Chapter = new FrontChapter(travel.CurrentChapter, true);
            }
        }

        public FrontBook Book { get; set; }
        public FrontChapter Chapter { get; set; }

        public override Travel ToDbModel()
        {
            throw new NotImplementedException();
        }
    }
}
