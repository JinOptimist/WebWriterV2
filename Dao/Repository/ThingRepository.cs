using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class ThingRepository : BaseRepository<Thing>, IThingRepository
    {
        private readonly ThingSampleRepository _thingSampleRepository;
        public ThingRepository(WriterContext db) : base(db)
        {
            _thingSampleRepository = new ThingSampleRepository(db);
        }

        public void CheckAndSave(Thing thing)
        {
            if (thing.ThingSample.Id == 0)
            {
                var thingSample = _thingSampleRepository.Entity.FirstOrDefault(x => x.Name == thing.ThingSample.Name);
                if (thingSample != null)
                {
                    thing.ThingSample = thingSample;
                }
            }

            Save(thing);
        }
    }
}