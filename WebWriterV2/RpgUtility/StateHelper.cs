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

            /* Set Race specification */
            switch (hero.Race)
            {
                case Race.Человек:
                    {
                        hero.AddNumberToState(maxHp, 5);
                        hero.AddNumberToState(maxMp, 5);
                        hero.AddNumberToState(dodge, 5);
                        hero.AddNumberToState(armor, 1);
                        hero.AddNumberToState(damage, 5);
                        break;
                    }
                case Race.Эльф:
                    {
                        hero.AddNumberToState(maxHp, 2);
                        hero.AddNumberToState(maxMp, 10);
                        hero.AddNumberToState(dodge, 15);
                        hero.AddNumberToState(armor, 1);
                        hero.AddNumberToState(damage, 5);
                        break;
                    }
                case Race.Орк:
                    {
                        hero.AddNumberToState(maxHp, 20);
                        hero.AddNumberToState(maxMp, -5);
                        hero.AddNumberToState(dodge, -5);
                        hero.AddNumberToState(armor, 1);
                        hero.AddNumberToState(damage, 5);
                        break;
                    }
                case Race.Гном:
                    {
                        hero.AddNumberToState(maxHp, 10);
                        hero.AddNumberToState(maxMp, -5);
                        hero.AddNumberToState(dodge, -5);
                        hero.AddNumberToState(armor, 1);
                        hero.AddNumberToState(damage, 5);
                        break;
                    }
                case Race.Дракон:
                    {
                        hero.AddNumberToState(maxHp, 20);
                        hero.AddNumberToState(maxMp, 2);
                        hero.AddNumberToState(dodge, 0);
                        hero.AddNumberToState(armor, 1);
                        hero.AddNumberToState(damage, 5);
                        break;
                    }
                default:
                    {
                        hero.AddNumberToState(maxHp, 5);
                        hero.AddNumberToState(maxMp, 5);
                        hero.AddNumberToState(dodge, 5);
                        hero.AddNumberToState(armor, 1);
                        hero.AddNumberToState(damage, 5);
                        break;
                    }
            }

            /* Set Sex specification */
            switch (hero.Sex)
            {
                case Sex.Жен:
                    {
                        hero.AddNumberToState(maxHp, 5);
                        hero.AddNumberToState(maxMp, 10);
                        hero.AddNumberToState(dodge, 10);
                        hero.AddNumberToState(armor, 1);
                        hero.AddNumberToState(damage, 5);
                        break;
                    }
                case Sex.Муж:
                    {
                        hero.AddNumberToState(maxHp, 10);
                        hero.AddNumberToState(maxMp, 5);
                        hero.AddNumberToState(dodge, 5);
                        hero.AddNumberToState(armor, 1);
                        hero.AddNumberToState(damage, 5);
                        break;
                    }
                case Sex.Скрывает:
                    {
                        hero.AddNumberToState(maxHp, 7);
                        hero.AddNumberToState(maxMp, 7);
                        hero.AddNumberToState(dodge, 15);
                        hero.AddNumberToState(armor, 1);
                        hero.AddNumberToState(damage, 5);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (var characteristic in hero.Characteristics)
            {
                characteristic.CharacteristicType.EffectState.ForEach(state =>
                    hero.AddNumberToState(state.StateType, state.Number * characteristic.Number));
            }

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