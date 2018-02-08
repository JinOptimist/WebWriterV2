using System.Collections.Generic;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class TravelRepository : BaseRepository<Travel>, ITravelRepository
    {
        public TravelRepository(WriterContext db) : base(db)
        {
        }

        public Travel GetByBookAndUser(long bookId, long userId)
        {
            return Entity.SingleOrDefault(x => x.Book.Id == bookId && x.Reader.Id == userId);
        }

        public List<Travel> GetByUser(long userId)
        {
            return Entity.Where(x => x.Reader.Id == userId).ToList();
        }
    }
}