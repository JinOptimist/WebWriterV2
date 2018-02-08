using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontEvaluation : BaseFront<Evaluation>
    {
        public FrontEvaluation()
        {
        }

        public FrontEvaluation(Evaluation evaluation)
        {
            Id = evaluation.Id;
            Comment = evaluation.Comment;
            Mark = evaluation.Mark;

            OwnerId = evaluation.Owner?.Id ?? -1;
            BookId = evaluation.Book?.Id ?? -1;
        }

        public long Mark { get; set; }
        public string Comment { get; set; }

        public long OwnerId { get; set; }
        public long BookId { get; set; }

        public override Evaluation ToDbModel()
        {
            return new Evaluation
            {
                Id = Id,
                Mark = Mark,
                Comment = Comment,
                Book = new Book { Id = BookId },
                Owner = new User { Id = OwnerId }
            };
        }
    }
}
