namespace Dao.Model
{
    public class FriendId
    {
        public virtual long Id { get; set; }

        public virtual UserFromVk UserFromVk { get; set; }

        public virtual long FriendsId { get; set; }
    }
}