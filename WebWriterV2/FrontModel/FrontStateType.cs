using System.Collections.Generic;
using System.Linq;
using Dao.Model;

namespace WebWriterV2.FrontModels
{
    public class FrontStateType : BaseFront<StateType>
    {
        public FrontStateType()
        {
        }

        public FrontStateType(StateType stateType)
        {
            Id = stateType.Id;
            Name = stateType.Name;
            Desc = stateType.Desc;
            BasicType = stateType.BasicType;
            BasicTypeName = stateType.BasicType.ToString();
            HideFromReader = stateType.HideFromReader;
            OwnerId = stateType.Owner?.Id;
            BookId = stateType.Book.Id;
        }

        public string Name { get; set; }
        public string Desc { get; set; }
        public StateBasicType BasicType { get; set; }
        public string BasicTypeName { get; set; }
        public bool HideFromReader { get; set; }
        public long? OwnerId { get; set; }
        public long BookId { get; set; }

        public override StateType ToDbModel()
        {
            return new StateType {
                Id = Id,
                Name = Name,
                Desc = Desc,
                Book = new Book { Id = BookId },
                BasicType = BasicType,
                HideFromReader = HideFromReader,
                Owner = OwnerId.HasValue ? new User { Id = OwnerId.Value } : null
            };
        }
    }
}
