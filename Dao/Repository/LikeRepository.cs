using System.Linq;
using Dal.Repository;
using Dal.IRepository;
using Dal.Model;

namespace Dal.Repository
{
    public class LikeRepository : BaseRepository<Like>, ILikeRepository
    {
        public LikeRepository(WriterContext db) : base(db)
        {
        }
    }
}