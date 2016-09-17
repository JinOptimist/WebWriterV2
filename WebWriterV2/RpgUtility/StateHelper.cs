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

            var gold = stateTypes.First(x => x.Name == GenerateData.Gold);
            var dodge = stateTypes.First(x => x.Name == GenerateData.Dodge);

            /* Set zeroes */
            hero.State = stateTypes.Select(stateType => new State { StateType = stateType, Number = 0 }).ToList();

            /* Set Race specification */
            switch (hero.Race)
            {
                case Race.Человек:
                    {
                        ChangeOneState(hero, maxHp, 5);
                        ChangeOneState(hero, maxMp, 5);
                        ChangeOneState(hero, dodge, 5);
                        ChangeOneState(hero, gold, 10);
                        break;
                    }
                case Race.Эльф:
                    {
                        ChangeOneState(hero, maxHp, 2);
                        ChangeOneState(hero, maxMp, 10);
                        ChangeOneState(hero, dodge, 15);
                        ChangeOneState(hero, gold, 10);
                        break;
                    }
                case Race.Орк:
                    {
                        ChangeOneState(hero, maxHp, 20);
                        ChangeOneState(hero, maxMp, -5);
                        ChangeOneState(hero, dodge, -5);
                        ChangeOneState(hero, gold, 0);
                        break;
                    }
                case Race.Гном:
                    {
                        ChangeOneState(hero, maxHp, 10);
                        ChangeOneState(hero, maxMp, -5);
                        ChangeOneState(hero, dodge, -5);
                        ChangeOneState(hero, gold, 50);
                        break;
                    }
                case Race.Дракон:
                    {
                        ChangeOneState(hero, maxHp, 20);
                        ChangeOneState(hero, maxMp, 2);
                        ChangeOneState(hero, dodge, 0);
                        ChangeOneState(hero, gold, 100);
                        break;
                    }
                default:
                    {
                        ChangeOneState(hero, maxHp, 5);
                        ChangeOneState(hero, maxMp, 5);
                        ChangeOneState(hero, dodge, 5);
                        ChangeOneState(hero, gold, 5);
                        break;
                    }
            }

            /* Set Sex specification */
            switch (hero.Sex)
            {
                case Sex.Жен:
                    {
                        ChangeOneState(hero, maxHp, 5);
                        ChangeOneState(hero, maxMp, 10);
                        ChangeOneState(hero, dodge, 10);
                        ChangeOneState(hero, gold, 10);
                        break;
                    }
                case Sex.Муж:
                    {
                        ChangeOneState(hero, maxHp, 10);
                        ChangeOneState(hero, maxMp, 5);
                        ChangeOneState(hero, dodge, 5);
                        ChangeOneState(hero, gold, 5);
                        break;
                    }
                case Sex.Скрывает:
                    {
                        ChangeOneState(hero, maxHp, 7);
                        ChangeOneState(hero, maxMp, 7);
                        ChangeOneState(hero, dodge, 15);
                        ChangeOneState(hero, gold, 20);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (var characteristic in hero.Characteristics)
            {
                characteristic.CharacteristicType.EffectState.ForEach(x => ChangeOneState(hero, x.StateType, x.Number));
            }

            var maxHpNumber = hero.State.First(x => x.StateType == maxHp).Number;
            var maxMpNumber = hero.State.First(x => x.StateType == maxMp).Number;

            hero.State.First(x => x.StateType == hp).Number = maxHpNumber;
            hero.State.First(x => x.StateType == mp).Number = maxMpNumber;
        }

        public static void ChangeOneState(this Hero hero, StateType stateType, long addingNumber)
        {
            hero.State.First(x => x.StateType == stateType).Number += addingNumber;
        }
    }
}