using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IUserRepository
    {
        UserFromVk GetUserFromVk(long id);

        List<UserFromVk> GetAllUserFromVk(int maxResult = 10);

        void RemoveAllUserFromVk();

        void SaveUserFromVk(UserFromVk userFromVk);

        bool ExistVkUser(long vkId);

        int CountUsers();
    }
}