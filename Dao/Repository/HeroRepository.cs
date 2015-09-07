using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class HeroRepository : IHeroRepository
    {
        private readonly WriterContext _db = new WriterContext();

        public Hero GetHero(long id)
        {
            return _db.Hero.SingleOrDefault(x => x.Id == id);
        }

        public List<Hero> GetAllHeroes()
        {
            return _db.Hero.ToList();
        }

        public void SaveHero(Hero hero)
        {
            if (hero.Id > 0)
            {
                _db.Hero.Attach(hero);
                _db.Entry(hero).State = EntityState.Modified;
                _db.SaveChanges();
                return;
            }

            _db.Hero.Add(hero);
            _db.SaveChanges();
        }
    }
}