using System.Linq;
using Dal.IRepository;
using Dal.Model;

namespace Dal.Repository
{
    public class ArticleRepository : BaseRepository<Article>, IArticleRepository
    {
        public ArticleRepository(WriterContext db) : base(db)
        {
        }
    }
}