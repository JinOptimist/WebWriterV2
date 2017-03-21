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

        [Description("Стартовый эвент")]
        public virtual Event RootEvent { get; set; }

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

            //TODO Update collection element by element
            //RootEvent = model.RootEvent;
            //Tips = model.Tips;
            //AllEvents = model.AllEvents;
        }
    }
}

