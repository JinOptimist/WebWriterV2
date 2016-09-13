using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class CharacteristicRepository : BaseRepository<Characteristic>, ICharacteristicRepository
    {
        public CharacteristicRepository(WriterContext db) : base(db)
        {
        }
    }
}