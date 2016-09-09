using System.Collections.Generic;
using System.Data;
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

        public override void Save(Hero model)
        {
            var skills = model.Skills.Select(x => x.Name).ToList();
            if (skills.Distinct().Count() != skills.Count)
                throw new DuplicateNameException("Hero can not has duplication in skills");
            base.Save(model);
        }

        public override List<Hero> GetAll()
        {
            return Entity.Include(x => x.Skills).Include(x => x.Characteristics).Include(x => x.State).ToList();
        }

        public override Hero Get(long id)
        {
            return Entity.Include(x => x.Skills).Include(x => x.Characteristics).Include(x => x.State).FirstOrDefault(x => x.Id == id);
        }

        public override List<Hero> GetList(IEnumerable<long> ids)
        {
            return Entity.Include(x => x.Skills).Include(x => x.Characteristics).Include(x => x.State).ToList();
        }
    }
}