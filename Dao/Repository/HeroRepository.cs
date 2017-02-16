using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class HeroRepository : BaseRepository<Hero>, IHeroRepository
    {
        private readonly ISkillRepository _skillRepository;
        private readonly ICharacteristicTypeRepository _characteristicTypeRepository;
        private readonly IStateTypeRepository _stateTypeRepository;

        public HeroRepository(WriterContext db) : base(db)
        {
            _skillRepository = new SkillRepository(db);
            _characteristicTypeRepository = new CharacteristicTypeRepository(db);
            _stateTypeRepository = new StateTypeRepository(db);
        }

        public override Hero Save(Hero hero)
        {
            var cleareSkill = hero.Skills
                .Where(skill => _skillRepository.Db.Entry(skill).State == EntityState.Detached)
                .Select(skill => new Skill { Id = skill.Id }).ToList();
            foreach (var skill in cleareSkill)
            {
                _skillRepository.Entity.Attach(skill);
                var item = hero.Skills.First(x => x.Id == skill.Id);
                hero.Skills.Remove(item);
                hero.Skills.Add(skill);
            }

            var skillIds = hero.Skills.Select(x => x.Id).ToList();
            if (skillIds.Distinct().Count() != skillIds.Count)
                throw new DuplicateNameException("Hero can not has duplication in skills");


            foreach (var characteristic in hero.Characteristics)
            {
                var isDetached = _characteristicTypeRepository.Db.Entry(characteristic.CharacteristicType).State == EntityState.Detached;
                if (isDetached)
                {
                    characteristic.CharacteristicType = new CharacteristicType { Id = characteristic.CharacteristicType.Id };
                    _characteristicTypeRepository.Entity.Attach(characteristic.CharacteristicType);
                }
            }

            foreach (var state in hero.State)
            {
                var isDetached = _stateTypeRepository.Db.Entry(state.StateType).State == EntityState.Detached;
                if (isDetached)
                {
                    state.StateType = new StateType { Id = state.StateType.Id };
                    _stateTypeRepository.Entity.Attach(state.StateType);
                }
            }

            if (hero.TimeCreation == default(DateTime))
            {
                hero.TimeCreation = DateTime.Now;
            }

            hero.LastChanges = DateTime.Now;

            base.Save(hero);
            return hero;
        }

        public List<Hero> GetByEvent(long eventId)
        {
            return Entity.Where(x => x.CurrentEvent.Id == eventId).ToList();
        }

        public void RemoveByEvent(long eventId, long userId)
        {
            var heroes = Entity.Where(x => x.CurrentEvent.Id == eventId && x.Owner.Id == userId).ToList();
            Remove(heroes);
        }

        public void RemoveByQuest(long questId, long userId)
        {
            var heroes = Entity.Where(x => x.CurrentEvent.Quest.Id == questId && x.Owner.Id == userId).ToList();
            Remove(heroes);
        }
    }
}