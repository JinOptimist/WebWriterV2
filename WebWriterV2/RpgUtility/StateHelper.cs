using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.RpgUtility
{
    public static class StateHelper
    {
        public static void SetDefaultState(this Hero hero)
        {
            hero.State = new List<State>
            {
                new State {StateType = StateType.MaxHp, Number = 0},
                new State {StateType = StateType.MaxMp, Number = 0},
                new State {StateType = StateType.Dodge, Number = 0},
                new State {StateType = StateType.Gold, Number = 0}
            };


            switch (hero.Sex)
            {
                case Sex.Жен:
                    {
                        ChangeOneState(hero, StateType.MaxHp, 5);
                        ChangeOneState(hero, StateType.MaxMp, 10);
                        ChangeOneState(hero, StateType.Dodge, 10);
                        ChangeOneState(hero, StateType.Gold, 10);
                        break;
                    }
                case Sex.Муж:
                    {
                        ChangeOneState(hero, StateType.MaxHp, 10);
                        ChangeOneState(hero, StateType.MaxMp, 5);
                        ChangeOneState(hero, StateType.Dodge, 5);
                        ChangeOneState(hero, StateType.Gold, 5);
                        break;
                    }
                case Sex.Скрывает:
                    {
                        ChangeOneState(hero, StateType.MaxHp, 7);
                        ChangeOneState(hero, StateType.MaxMp, 7);
                        ChangeOneState(hero, StateType.Dodge, 15);
                        ChangeOneState(hero, StateType.Gold, 20);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (hero.Race)
            {
                case Race.Человек:
                    {
                        ChangeOneState(hero, StateType.MaxHp, 5);
                        ChangeOneState(hero, StateType.MaxMp, 5);
                        ChangeOneState(hero, StateType.Dodge, 5);
                        ChangeOneState(hero, StateType.Gold, 10);
                        break;
                    }
                case Race.Эльф:
                    {
                        ChangeOneState(hero, StateType.MaxHp, 2);
                        ChangeOneState(hero, StateType.MaxMp, 10);
                        ChangeOneState(hero, StateType.Dodge, 15);
                        ChangeOneState(hero, StateType.Gold, 10);
                        break;
                    }
                case Race.Орк:
                    {
                        ChangeOneState(hero, StateType.MaxHp, 20);
                        ChangeOneState(hero, StateType.MaxMp, -5);
                        ChangeOneState(hero, StateType.Dodge, -5);
                        ChangeOneState(hero, StateType.Gold, 0);
                        break;
                    }
                case Race.Гном:
                    {
                        ChangeOneState(hero, StateType.MaxHp, 10);
                        ChangeOneState(hero, StateType.MaxMp, -5);
                        ChangeOneState(hero, StateType.Dodge, -5);
                        ChangeOneState(hero, StateType.Gold, 50);
                        break;
                    }
                case Race.Дракон:
                    {
                        ChangeOneState(hero, StateType.MaxHp, 20);
                        ChangeOneState(hero, StateType.MaxMp, 2);
                        ChangeOneState(hero, StateType.Dodge, 0);
                        ChangeOneState(hero, StateType.Gold, 100);
                        break;
                    }
                default:
                    {
                        ChangeOneState(hero, StateType.MaxHp, 5);
                        ChangeOneState(hero, StateType.MaxMp, 5);
                        ChangeOneState(hero, StateType.Dodge, 5);
                        ChangeOneState(hero, StateType.Gold, 5);
                        break;
                    }
            }

            foreach (var characteristic in hero.Characteristics)
            {
                switch (characteristic.CharacteristicType)
                {
                    case CharacteristicType.Strength:
                        {
                            ChangeOneState(hero, StateType.MaxHp, characteristic.Number * 2);
                            break;
                        }
                    case CharacteristicType.Agility:
                        {
                            ChangeOneState(hero, StateType.Dodge, characteristic.Number * 2);
                            break;
                        }
                    case CharacteristicType.Charism:
                        {
                            ChangeOneState(hero, StateType.Gold, characteristic.Number * 2);
                            break;
                        }
                }
            }

            var maxHp = hero.State.First(x => x.StateType == StateType.MaxHp).Number;
            var maxMp = hero.State.First(x => x.StateType == StateType.MaxMp).Number;
            hero.State.Add(new State { StateType = StateType.CurrentHp, Number = maxHp });
            hero.State.Add(new State { StateType = StateType.CurrentMp, Number = maxMp });
        }

        public static void ChangeOneState(this Hero hero, StateType stateType, long addingNumber)
        {
            hero.State.First(x => x.StateType == stateType).Number += addingNumber;
        }
    }
}