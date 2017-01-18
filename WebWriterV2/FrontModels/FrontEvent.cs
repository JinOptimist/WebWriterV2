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
            RequirementRace = new FrontEnum(eventDb.RequirementRace);
            RequirementSex = new FrontEnum(eventDb.RequirementSex);
            LinksFromThisEvent = eventDb.LinksFromThisEvent?.Select(x => new FrontEventLinkItem(x)).ToList();
            LinksToThisEvent = eventDb.LinksToThisEvent?.Select(x => new FrontEventLinkItem(x)).ToList();
            ProgressChanging = eventDb.ProgressChanging;
            RequirementSkill = eventDb.RequirementSkill?.Select(x => new FrontSkill(x)).ToList();
            RequirementCharacteristics = eventDb.RequirementCharacteristics?.Select(x => new FrontCharacteristic(x)).ToList();
            HeroStatesChanging = eventDb.HeroStatesChanging?.Select(x => new FrontState(x)).ToList();
            CharacteristicsChanges = eventDb.CharacteristicsChanges?.Select(x => new FrontCharacteristic(x)).ToList();
            ThingsChanges = eventDb.ThingsChanges?.Select(x => new FrontThing(x)).ToList();
            RequirementThings = eventDb.RequirementThings?.Select(x => new FrontThing(x)).ToList();
            RequirementState = eventDb.RequirementStates?.Select(x => new FrontState(x)).ToList();
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public FrontEnum RequirementRace { get; set; }
        public FrontEnum RequirementSex { get; set; }
        public List<FrontEventLinkItem> LinksFromThisEvent { get; set; }
        public List<FrontEventLinkItem> LinksToThisEvent { get; set; }
        public List<FrontSkill> RequirementSkill { get; set; }
        public List<FrontCharacteristic> RequirementCharacteristics { get; set; }
        public List<FrontState> RequirementState { get; set; }
        public List<FrontThing> RequirementThings { get; set; }
        public List<FrontCharacteristic> CharacteristicsChanges { get; set; }
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
                RequirementRace = (Race?)RequirementRace?.Value,
                RequirementSex = (Sex?)RequirementSex?.Value,
                LinksFromThisEvent = LinksFromThisEvent?.Select(x => x.ToDbModel()).ToList(),
                LinksToThisEvent = LinksToThisEvent?.Select(x => x.ToDbModel()).ToList(),
                ProgressChanging = ProgressChanging,

                RequirementSkill = RequirementSkill?.Select(x => x.ToDbModel()).ToList(),
                RequirementThings = RequirementThings?.Select(x => x.ToDbModel()).ToList(),
                RequirementCharacteristics = RequirementCharacteristics?.Select(x => x.ToDbModel()).ToList(),
                RequirementStates = RequirementState?.Select(x => x.ToDbModel()).ToList(),
                CharacteristicsChanges = CharacteristicsChanges?.Select(x => x.ToDbModel()).ToList(),
                ThingsChanges = ThingsChanges?.Select(x => x.ToDbModel()).ToList(),
                HeroStatesChanging = HeroStatesChanging?.Select(x => x.ToDbModel()).ToList()
            };
        }
    }
}