using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dao.Model
{
    public class Genre : BaseModel
    {
        public string Name { get; set; }

        public string Desc { get; set; }

        public virtual List<Quest> Quests { get; set; }
    }
}
