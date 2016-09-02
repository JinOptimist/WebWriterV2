using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebWriterV2.Models.rpg
{
    public class Quest : BaseModel
    {
        public string Name { get; set; }
        public string Desc { get; set; }


        /// <summary>
        /// Эффективность с которой был выполнен квест. При закрытие квеста, умножаем базовую награду на этот коэффицент
        /// </summary>
        public double Effective { get; set; } = 0;// [1.0 = 100%]

        /// <summary>
        /// Базовая награда. Меняет статус героя, добавляя ему опыт, деньги, жизни и т.д.
        /// </summary>
        public Dictionary<CharacteristicType, int> Result { get; set; }

        public Hero Executor { get; set; }

        public Event RootEvent { get; set; }

        /// <summary>
        /// Список подзказок. Подсказки можно покупать, что бы узнать о том что тебя ждёт
        /// </summary>
        public List<string> Tips { get; set; } = new List<string>();

        /// <summary>
        /// Подзадача которая сейчас выполняется героем
        /// </summary>
        public Event CurentEvent { get; set; }
        public List<Event> History { get; set; } = new List<Event>();
    }
}

