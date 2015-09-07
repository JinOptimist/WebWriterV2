using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dao.Model
{
    public class UserFromVk
    {
        public virtual long Id { get; set; }

        public virtual long VkId { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual EnumSex Sex { get; set; }

        public virtual string Nickname { get; set; }

        public virtual DateTime AddedToMyBase { get; set; }

        public virtual List<FriendId> FriendIds { get; set; }

        public override string ToString()
        {
            return string.Format("Inner Id:{0} VkId: {1} Name: {2} {3} Sex: {4} AddedToMyBase: {5} Friend ids: {6}",
                Id, VkId, FirstName, LastName, Sex, AddedToMyBase.ToLongDateString(),
                FriendIds != null && FriendIds.Any()
                    ? FriendIds.Select(x => x.FriendsId).Select(x => x.ToString()).Aggregate((i, j) => i + " " + j)
                    : string.Empty);
        }
    }

    public enum EnumSex
    {
        Woman = 1,
        Man = 2
    }
}