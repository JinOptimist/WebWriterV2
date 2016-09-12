using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class TrainingRoomRepository : BaseRepository<TrainingRoom>, ITrainingRoomRepository
    {
        public TrainingRoomRepository(WriterContext db) : base(db)
        {
        }
    }
}