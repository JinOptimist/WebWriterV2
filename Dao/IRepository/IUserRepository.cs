﻿using System.Collections.Generic;
using Dao.Model;

namespace Dao.IRepository
{
    public interface IUserRepository
    {
        UserFromVk GetUserVk(long id);

        UserFromVk GetUserByVkId(long vkId);

        /// <summary>
        /// Search in FriendId table
        /// </summary>
        /// <returns>VkId</returns>
        long GetUnsaveUserVkId();

        List<UserFromVk> GetAllUserFromVk(int maxResult = 10);

        void RemoveAllUserFromVk();

        void SaveUserFromVk(UserFromVk userFromVk);

        bool ExistVkUser(long vkId);

        int CountUsers();
    }
}