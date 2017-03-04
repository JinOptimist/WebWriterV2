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
            Password = user.Password;

            UserType = new FrontEnum(user.UserType);

            IsAdmin = user.UserType == Dao.Model.UserType.Admin;
            IsWriter = user.UserType == Dao.Model.UserType.Writer;
            Bookmarks = user.Bookmarks?.Select(x => new FrontHero(x)).ToList();
            BooksAreReaded = user.BooksAreReaded?.Select(x => new FrontQuest(x)).ToList();
        }

        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsWriter { get; set; }

        public FrontEnum UserType { get; set; }

        public List<FrontHero> Bookmarks { get; set; }
        public List<FrontQuest> BooksAreReaded { get; set; }

        public override User ToDbModel()
        {
            var user = new User
            {
                Id = Id,
                Name = Name,
                Password = Password,
                Email = Email,
                UserType = IsAdmin
            };

            return user;
        }
    }
}