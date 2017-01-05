using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class SkillRepository : BaseRepository<Skill>, ISkillRepository
    {
        private readonly IStateTypeRepository _stateTypeRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ISkillSchoolRepository _skillSchoolRepository;

        public SkillRepository(WriterContext db) : base(db)
        {
            _stateTypeRepository = new StateTypeRepository(db);
            _skillSchoolRepository = new SkillSchoolRepository(db);
            _stateRepository = new StateRepository(db);
        }

        public override Skill Save(Skill skill)
        {
            var isDetached = true;
            var skillByName = GetByName(skill.Name);
            if (skillByName != null && skillByName.Id != skill.Id)
            {
                throw new DuplicateNameException("Skill cann't has duplication in name");
            }

            skill.SelfChanging = skill.SelfChanging ?? new List<State>();
            foreach (var state in skill.SelfChanging)
            {
                isDetached = _stateTypeRepository.Db.Entry(state.StateType).State == EntityState.Detached;
                if (isDetached)
                {
                    state.StateType = new StateType { Id = state.StateType.Id };
                    _stateTypeRepository.Entity.Attach(state.StateType);
                }
            }

            skill.TargetChanging = skill.TargetChanging ?? new List<State>();
            foreach (var state in skill.TargetChanging)
            {
                isDetached = _stateTypeRepository.Db.Entry(state.StateType).State == EntityState.Detached;
                if (isDetached)
                {
                    state.StateType = new StateType { Id = state.StateType.Id };
                    _stateTypeRepository.Entity.Attach(state.StateType);
                }
            }

            isDetached = _skillSchoolRepository.Db.Entry(skill.School).State == EntityState.Detached;
            if (isDetached)
            {
                skill.School = new SkillSchool { Id = skill.School.Id };
                _skillSchoolRepository.Entity.Attach(skill.School);
            }

            return base.Save(skill);
        }

        public override bool Exist(Skill skill)
        {
            return Entity.Any(x => x.Name == skill.Name);
        }

        public override void Remove(Skill baseModel)
        {
            _stateRepository.Remove(baseModel.SelfChanging);

            base.Remove(baseModel);
        }

        public Skill GetByName(string skillName)
        {
            var nbsp = (char)160;// code of nbsp
            if (skillName.Contains(nbsp))
            {
                throw  new Exception("You use nbsp between simple space. This is a problem");
            }
            return Entity.FirstOrDefault(x => x.Name == skillName);
        }

        public List<Skill> GetBySchool(SkillSchool skillSchool)
        {
            return Entity.Where(x => x.School.Id == skillSchool.Id).ToList();
        }

        public List<Skill> GetBySchoolName(string schoolName)
        {
            return Entity.Where(x => x.School.Name == schoolName).ToList();
        }

        public Dictionary<SkillSchool, List<Skill>> GetBySchools(List<SkillSchool> skillSchool)
        {
            var result = new Dictionary<SkillSchool, List<Skill>>();

            foreach (var school in skillSchool)
            {
                var skills = GetBySchool(school);
                result.Add(school, skills);
            }

            return result;
        }

        public Skill CheckAndSave(Skill skill)
        {
            var skillByName = GetByName(skill.Name);
            if (skillByName != null && skillByName.Id != skill.Id)
            {
                return skillByName;
            }

            if (skill.School.Id != 0)
                return skill;


            var skillSchool = _skillSchoolRepository.Entity.FirstOrDefault(x => x.Name == skill.School.Name);
            if (skillSchool != null)
            {
                skill.School = skillSchool;
            }
            return Save(skill);
        }
    }
}