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
            IsReader = user.UserType == Dao.Model.UserType.Reader;
            AvatarUrl = user.AvatarUrl;
            Bookmarks = user.Bookmarks?.Select(x => new FrontHero(x)).ToList();
            BooksAreReaded = user.BooksAreReaded?.Select(x => new FrontBook(x)).ToList();
            user.Books?.ForEach(x => x.Owner = null);
            MyBooks = user.Books?.Select(x => new FrontBook(x)).ToList();
            AccountConfirmed = string.IsNullOrEmpty(user.ConfirmCode);
        }

        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsWriter { get; set; }
        public bool IsReader { get; set; }
        public bool AccountConfirmed { get; set; }
        public string AvatarUrl { get; set; }

        public FrontEnum UserType { get; set; }

        public List<FrontHero> Bookmarks { get; set; }
        public List<FrontBook> BooksAreReaded { get; set; }
        public List<FrontBook> MyBooks { get; set; }

        public override User ToDbModel()
        {
            var user = new User
            {
                Id = Id,
                Name = Name,
                Password = Password,
                Email = Email,
                UserType = UserType != null ? (UserType)UserType.Value : Dao.Model.UserType.Reader,
                AvatarUrl = AvatarUrl
            };

            return user;
        }
    }
}