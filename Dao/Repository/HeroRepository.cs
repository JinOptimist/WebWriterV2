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
    }
}