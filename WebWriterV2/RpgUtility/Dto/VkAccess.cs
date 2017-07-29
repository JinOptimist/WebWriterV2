using System;
using System.Runtime.Serialization;

namespace WebWriterV2.RpgUtility.Dto
{
    [DataContract]
    [Serializable]
    public class VkAccess
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "expires_in")]
        public long ExpiresIn { get; set; }

        [DataMember(Name = "user_id")]
        public long UserId { get; set; }
    }
}
