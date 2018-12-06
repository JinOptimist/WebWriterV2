using System.Linq;
using Dal.IRepository;
using Dal.Model;

namespace Dal.Repository
{
    public class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        public GenreRepository(WriterContext db) : base(db)
        {
        }
    }
}