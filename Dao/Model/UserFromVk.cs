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
            return
                $"Inner Id:{Id} VkId: {VkId} Name: {FirstName} {LastName} Sex: {Sex} AddedToMyBase: {AddedToMyBase.ToLongDateString()} Friend ids: {(FriendIds != null && FriendIds.Any() ? FriendIds.Select(x => x.FriendsId).Select(x => x.ToString()).Aggregate((i, j) => i + " " + j) : string.Empty)}";
        }
    }

    public enum EnumSex
    {
        Woman = 1,
        Man = 2
    }
}