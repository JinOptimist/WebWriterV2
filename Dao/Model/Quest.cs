using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dao.Model
{
    public class Quest : BaseModel, IUpdatable<Quest>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Desc { get; set; }

        [Required]
        [Description("[1.0 = 100%] Эффективность с которой был выполнен квест. При закрытие квеста, умножаем базовую награду на этот коэффицент")]
        public double Effective { get; set; } = 0;

        //[Description("Базовая награда. Меняет статус героя, добавляя ему опыт, деньги, жизни и т.д.")]
        //public virtual Dictionary<CharacteristicType, int> Result { get; set; }

        public virtual Hero Executor { get; set; }

        [Description("Стартовый эвент")]
        public virtual Event RootEvent { get; set; }

        [Description("Список подсказок. Подсказки можно покупать, что бы узнать о том что тебя ждёт")]
        public virtual List<string> Tips { get; set; } = new List<string>();

        //[Description("Подзадача которая сейчас выполняется героем")]
        //public Event CurentEvent { get; set; }

        //[Description("Список уже пройденных подзадача, чьи эффекты были премененны на героя и квест в целом")]
        //public List<Event> History { get; set; } = new List<Event>();

        public virtual List<Event> AllEvents { get; set; }

        public virtual List<Evaluation> Evaluations { get; set; }

        public virtual User Owner { get; set; }

        public void UpdateFrom(Quest model)
        {
            if (Id != model.Id)
            {
                throw new Exception($"You try update Quest model with id {Id} from model with id {Id}");
            }

            Name = model.Name;
            Desc = model.Desc;
            Effective = model.Effective;
            Executor = model.Executor;

            //TODO Update collection element by element
            //RootEvent = model.RootEvent;
            //Tips = model.Tips;
            //AllEvents = model.AllEvents;
        }
    }
}

