using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class TravelStepRepository : BaseRepository<TravelStep>, ITravelStepRepository
    {
        public TravelStepRepository(WriterContext db) : base(db)
        {
        }
    }
}