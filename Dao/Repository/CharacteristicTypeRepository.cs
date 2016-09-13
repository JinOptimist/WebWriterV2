using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class CharacteristicTypeRepository : BaseRepository<CharacteristicType>, ICharacteristicTypeRepository
    {
        public CharacteristicTypeRepository(WriterContext db) : base(db)
        {
        }
    }
}