using System;
using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontEvent : BaseFront<Event>
    {
        public FrontEvent()
        {
        }

        public FrontEvent(Event eventDb)
        {
            Id = eventDb.Id;
            Name = eventDb.Name;
            Desc = eventDb.Desc;
            RequrmentRace = new FronEnum(eventDb.RequrmentRace);
            RequrmentSex = new FronEnum(eventDb.RequrmentSex);
            LinksFromThisEvent = eventDb.LinksFromThisEvent?.Select(x => new FrontEventLinkItem(x)).ToList();
            LinksToThisEvent = eventDb.LinksToThisEvent?.Select(x => new FrontEventLinkItem(x)).ToList();
            ProgressChanging = eventDb.ProgressChanging;
            RequrmentSkill = eventDb.RequrmentSkill?.Select(x => new FrontSkill(x)).ToList();
            RequrmentCharacteristics = eventDb.RequrmentCharacteristics?.Select(x => new FrontCharacteristic(x)).ToList();
            HeroStatesChanging = eventDb.HeroStatesChanging?.Select(x => new FrontState(x)).ToList();
            ThingsChanges = eventDb.ThingsChanges?.Select(x => new FrontThing(x)).ToList();
            RequirementThings = eventDb.RequirementThings?.Select(x => new FrontThing(x)).ToList();
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public FronEnum RequrmentRace { get; set; }
        public FronEnum RequrmentSex { get; set; }
        public List<FrontEventLinkItem> LinksFromThisEvent { get; set; }
        public List<FrontEventLinkItem> LinksToThisEvent { get; set; }
        public List<FrontSkill> RequrmentSkill { get; set; }
        public List<FrontCharacteristic> RequrmentCharacteristics { get; set; }
        public List<FrontThing> RequirementThings { get; set; }
        public List<FrontThing> ThingsChanges { get; set; }
        public List<FrontState> HeroStatesChanging { get; set; }

        public double ProgressChanging { get; set; }

        public override Event ToDbModel()
        {
            return new Event
            {
                Id = Id,
                Name = Name,
                Desc = Desc,
                RequrmentRace = (Race?)RequrmentRace?.Value,
                RequrmentSex = (Sex?)RequrmentSex?.Value,
                LinksFromThisEvent = LinksFromThisEvent?.Select(x => x.ToDbModel()).ToList(),
                LinksToThisEvent = LinksToThisEvent?.Select(x => x.ToDbModel()).ToList(),
                ProgressChanging = ProgressChanging,
                //RequrmentSkill = RequrmentSkill?.Select(x => x.ToDbModel()).ToList(),
            };
        }
    }
}