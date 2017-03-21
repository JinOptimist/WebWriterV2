using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.RpgUtility
{
    public static class StateHelper
    {
        public static void SetDefaultState(this Hero hero, List<StateType> stateTypes)
        {
            var hp = stateTypes.First(x => x.Name == GenerateData.Hp);
            var maxHp = stateTypes.First(x => x.Name == GenerateData.MaxHp);
            var mp = stateTypes.First(x => x.Name == GenerateData.Mp);
            var maxMp = stateTypes.First(x => x.Name == GenerateData.MaxMp);

            //var gold = stateTypes.First(x => x.Name == GenerateData.Gold);
            var dodge = stateTypes.First(x => x.Name == GenerateData.Dodge);
            var armor = stateTypes.First(x => x.Name == GenerateData.Armor);
            var damage = stateTypes.First(x => x.Name == GenerateData.Damage);

            /* Set zeroes */
            hero.State = stateTypes.Select(stateType => new State { StateType = stateType, Number = 0 }).ToList();

            var maxHpNumber = hero.State.First(x => x.StateType == maxHp).Number;
            var maxMpNumber = hero.State.First(x => x.StateType == maxMp).Number;

            hero.State.First(x => x.StateType == hp).Number = maxHpNumber;
            hero.State.First(x => x.StateType == mp).Number = maxMpNumber;
        }

        public static void AddNumberToState(this Hero hero, StateType stateType, long addingNumber)
        {
            hero.State.First(x => x.StateType == stateType).Number += addingNumber;
        }
    }
}