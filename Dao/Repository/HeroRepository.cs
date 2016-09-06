using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class HeroRepository : BaseRepository<Hero>, IHeroRepository
    {
        public HeroRepository(WriterContext db) : base(db)
        {
        }

        public new List<Hero> GetAll()
        {
            return Entity.Include(x => x.Skills).Include(x => x.Characteristics).ToList();
        }
    }
}