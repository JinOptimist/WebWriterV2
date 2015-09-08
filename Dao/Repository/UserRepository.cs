using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dao.IRepository;
using Dao.Model;

namespace Dao.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly WriterContext _db = new WriterContext();

        public UserFromVk GetUserVk(long id)
        {
            return _db.UserFromVk.SingleOrDefault(x => x.Id == id);
        }

        public UserFromVk GetUserByVkId(long vkId)
        {
            return _db.UserFromVk.SingleOrDefault(x => x.VkId == vkId);
        }

        public List<UserFromVk> GetAllUserFromVk(int maxResult = 10)
        {
            return _db.UserFromVk.Take(maxResult).ToList();
        }

        public void RemoveAllUserFromVk()
        {
            _db.UserFromVk.RemoveRange(GetAllUserFromVk(int.MaxValue));
            _db.SaveChanges();
        }

        public void SaveUserFromVk(UserFromVk userFromVk)
        {
            if (userFromVk.Id > 0)
            {
                _db.UserFromVk.Attach(userFromVk);
                _db.Entry(userFromVk).State = EntityState.Modified;
                _db.SaveChanges();
                return;
            }

            lock (_db)
            {
                if (userFromVk.VkId > 0 && _db.UserFromVk.Any(x => x.VkId == userFromVk.VkId))
                {
                    var user = _db.UserFromVk.SingleOrDefault(x => x.VkId == userFromVk.VkId);
                    userFromVk = user;
                    return;
                }

                if (userFromVk.AddedToMyBase == DateTime.MinValue)
                {
                    userFromVk.AddedToMyBase = DateTime.Now;
                }

                _db.UserFromVk.Add(userFromVk);
                _db.SaveChanges();
            }
        }

        public bool ExistVkUser(long vkId)
        {
            return _db.UserFromVk.Any(x => x.VkId == vkId);
        }

        public int CountUsers()
        {
            return _db.UserFromVk.Count();
        }
    }
}