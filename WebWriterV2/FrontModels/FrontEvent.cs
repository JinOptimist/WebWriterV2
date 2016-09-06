using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontEvent
    {
        public FrontEvent()
        {
        }

        public FrontEvent(Event eventDb)
        {
            Id = eventDb.Id;
            Name = eventDb.Name;
            Desc = eventDb.Desc;
            ChildrenEvents = eventDb.ChildrenEvents?.Select(x => new FrontEvent(x)).ToList();
            ProgressChanging = eventDb.ProgressChanging;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public List<FrontEvent> ChildrenEvents { get; set; }
        public double ProgressChanging { get; set; } = 0;
    }
}