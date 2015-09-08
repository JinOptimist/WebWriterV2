using Dao.Model;

namespace WebWriterV2.GetUserFromJsonFile
{
    public class ExternalUserModel
    {
        public long UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public EnumSex Sex { get; set; }

        public string Nickname { get; set; }

        public long[] FriendIds { get; set; }
    }
}