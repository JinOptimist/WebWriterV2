using System.Collections.Generic;
using System.Linq;
using Dal.IRepository;
using Dal.Model;

namespace Dal.Repository
{
    public class TravelRepository : BaseRepository<Travel>, ITravelRepository
    {
        private IStateValueRepository _stateValueRepository;
        public TravelRepository(WriterContext db) : base(db)
        {
            _stateValueRepository = new StateValueRepository(db);
        }

        public Travel GetByBookAndUser(long bookId, long userId)
        {
            return Entity.SingleOrDefault(x => x.Book.Id == bookId && x.Reader.Id == userId);
        }

        public List<Travel> GetByUser(long userId)
        {
            return Entity.Where(x => x.Reader.Id == userId).ToList();
        }

        public override void Remove(Travel baseModel)
        {
            var states = baseModel.State;
            _stateValueRepository.Remove(states);

            base.Remove(baseModel);
        }
    }
}