using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dao.Model
{
    public class Travel : BaseModel
    {
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime? FinishTime { get; set; }

        public virtual User Reader { get; set; }
        public virtual Book Book { get; set; }

        public virtual Chapter CurrentChapter { get; set; }

        public virtual List<TravelStep> Steps { get; set; }

        public virtual bool IsTravelEnd { get; set; }

        /// <summary>
        /// For future. To implementation State
        /// </summary>
        public virtual List<StateValue> State { get; set; }
    }
}
