using System.Linq;
using Dao.Model;
using VkApi.DTO;

namespace WebWriterV2.VkUtility
{
    public static class UserConverter
    {
        public static UserFromVk GetUserFromVk(VkUser user)
        {
            var userFromVk = new UserFromVk
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Nickname = user.Nickname,
                Sex = user.Sex == Sex.Man ? EnumSex.Man : EnumSex.Woman,
                VkId = user.UserId
            };

            if (user.FriendIds != null && user.FriendIds.Any())
            {
                userFromVk.FriendIds =
                    user.FriendIds.Select(x => new FriendId {UserFromVk = userFromVk, FriendsId = x}).ToList();
            }

            return userFromVk;
        }
    }
}