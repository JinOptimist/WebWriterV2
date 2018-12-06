using System.Linq;
using Dal.IRepository;
using Dal.Model;

namespace Dal.Repository
{
    public class TravelStepRepository : BaseRepository<TravelStep>, ITravelStepRepository
    {
        public TravelStepRepository(WriterContext db) : base(db)
        {
        }
    }
}