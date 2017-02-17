using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Dao.Model;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.FrontModels
{
    public class FrontUser : BaseFront<User>
    {
        public FrontUser()
        {
        }

        public FrontUser(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            IsAdmin = user.IsAdmin;
            Password = user.Password;
            Bookmarks = user.Bookmarks?.Select(x => new FrontHero(x)).ToList();
        }

        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }

        public List<FrontHero> Bookmarks { get; set; }

        public override User ToDbModel()
        {
            var user = new User
            {
                Id = Id,
                Name = Name,
                Password = Password,
                Email = Email,
                IsAdmin = IsAdmin
            };

            return user;
        }
    }
}