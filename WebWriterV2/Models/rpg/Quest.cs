using System.Collections.Generic;

namespace WebWriterV2.Models.rpg
{
    public class Quest
    {
        public long Id { get; set; }
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

        public List<Event> QuestEvents { get; set; } = new List<Event>();

        /// <summary>
        /// Подзадача которая сейчас выполняется героем
        /// </summary>
        public Event CurentEvent { get; set; }
        public List<Event> History { get; set; } = new List<Event>();
    }
}
