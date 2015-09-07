using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IHeroRepository
    {
        Hero GetHero(long id);

        List<Hero> GetAllHeroes();

        void SaveHero(Hero hero);
    }
}