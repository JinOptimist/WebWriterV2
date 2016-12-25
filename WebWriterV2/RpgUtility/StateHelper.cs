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
                        AddNumberToState(hero, maxHp, 5);
                        AddNumberToState(hero, maxMp, 5);
                        AddNumberToState(hero, dodge, 5);
                        //ChangeOneState(hero, gold, 10);
                        AddNumberToState(hero, armor, 1);
                        AddNumberToState(hero, damage, 5);
                        break;
                    }
                case Race.Эльф:
                    {
                        AddNumberToState(hero, maxHp, 2);
                        AddNumberToState(hero, maxMp, 10);
                        AddNumberToState(hero, dodge, 15);
                        //ChangeOneState(hero, gold, 10);
                        AddNumberToState(hero, armor, 1);
                        AddNumberToState(hero, damage, 5);
                        break;
                    }
                case Race.Орк:
                    {
                        AddNumberToState(hero, maxHp, 20);
                        AddNumberToState(hero, maxMp, -5);
                        AddNumberToState(hero, dodge, -5);
                        //ChangeOneState(hero, gold, 0);
                        AddNumberToState(hero, armor, 1);
                        AddNumberToState(hero, damage, 5);
                        break;
                    }
                case Race.Гном:
                    {
                        AddNumberToState(hero, maxHp, 10);
                        AddNumberToState(hero, maxMp, -5);
                        AddNumberToState(hero, dodge, -5);
                        //ChangeOneState(hero, gold, 50);
                        AddNumberToState(hero, armor, 1);
                        AddNumberToState(hero, damage, 5);
                        break;
                    }
                case Race.Дракон:
                    {
                        AddNumberToState(hero, maxHp, 20);
                        AddNumberToState(hero, maxMp, 2);
                        AddNumberToState(hero, dodge, 0);
                        //ChangeOneState(hero, gold, 100);
                        AddNumberToState(hero, armor, 1);
                        AddNumberToState(hero, damage, 5);
                        break;
                    }
                default:
                    {
                        AddNumberToState(hero, maxHp, 5);
                        AddNumberToState(hero, maxMp, 5);
                        AddNumberToState(hero, dodge, 5);
                        //ChangeOneState(hero, gold, 5);
                        AddNumberToState(hero, armor, 1);
                        AddNumberToState(hero, damage, 5);
                        break;
                    }
            }

            /* Set Sex specification */
            switch (hero.Sex)
            {
                case Sex.Жен:
                    {
                        AddNumberToState(hero, maxHp, 5);
                        AddNumberToState(hero, maxMp, 10);
                        AddNumberToState(hero, dodge, 10);
                        //ChangeOneState(hero, gold, 10);
                        AddNumberToState(hero, armor, 1);
                        AddNumberToState(hero, damage, 5);
                        break;
                    }
                case Sex.Муж:
                    {
                        AddNumberToState(hero, maxHp, 10);
                        AddNumberToState(hero, maxMp, 5);
                        AddNumberToState(hero, dodge, 5);
                        //ChangeOneState(hero, gold, 5);
                        AddNumberToState(hero, armor, 1);
                        AddNumberToState(hero, damage, 5);
                        break;
                    }
                case Sex.Скрывает:
                    {
                        AddNumberToState(hero, maxHp, 7);
                        AddNumberToState(hero, maxMp, 7);
                        AddNumberToState(hero, dodge, 15);
                        //ChangeOneState(hero, gold, 20);
                        AddNumberToState(hero, armor, 1);
                        AddNumberToState(hero, damage, 5);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (var characteristic in hero.Characteristics)
            {
                characteristic.CharacteristicType.EffectState.ForEach(x => AddNumberToState(hero, x.StateType, x.Number));
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