using System.Linq;
using Dao.Model;
using VkApi.Web;

namespace WebWriterV2.VkUtility
{
    public static class Downloader
    {
        public static UserFromVk Download(long vkId)
        {
            var vkApiCaller = new VkApiCaller();
            var vkUser = vkApiCaller.GetVkUser(vkId);
            if (vkUser == null)
            {
                return null;
            }

            var friends = vkApiCaller.GetUserFriends(vkId);
            if (friends != null)
            {
                vkUser.FriendIds = friends.ToList();
            }

            var userFromVk = UserConverter.GetUserFromVk(vkUser);
            return userFromVk;
        }
    }
}