using System.Collections.Generic;
using System.Threading.Tasks;
using Dao.Model;
using WebWriterV2.SecondThread;

namespace WebWriterV2.Models
{
    public class MarkViewModel
    {
        public int TotalUserFromDb { get; set; }

        public int TotalFriendFromDb { get; set; }

        public UserFromVk CurrentVkUser { get; set; }

        public List<FriendWithState> Friends { get; set; }

        public TaskStatus TaskStatus { get; set; }
    }
}