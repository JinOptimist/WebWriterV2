using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Dal.Model;
using WebWriterV2.RpgUtility;
using DalUserType = Dal.Model.UserType;

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

            IsAdmin = user.UserType == DalUserType.Admin;
            IsWriter = user.UserType == DalUserType.Writer || user.UserType == DalUserType.Admin;
            IsReader = user.UserType == DalUserType.Reader;
            AvatarUrl = user.AvatarUrl;
            //Bookmarks = user.Bookmarks?.Select(x => new FrontHero(x)).ToList();
            ReadedBookIds = user.BooksAreReaded?.Select(x => x.Book.Id).ToList();
            //MyBooks = user.Books?.Select(x => new FrontBook(x)).ToList();
            AccountConfirmed = string.IsNullOrEmpty(user.ConfirmCode);

            ShowExtendedFunctionality = user.ShowExtendedFunctionality;
        }

        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsWriter { get; set; }
        public bool IsReader { get; set; }
        public bool AccountConfirmed { get; set; }
        public bool ShowExtendedFunctionality { get; set; }
        public string AvatarUrl { get; set; }

        public string Error { get; set; }

        public FrontEnum UserType { get; set; }

        //public List<FrontHero> Bookmarks { get; set; }
        public List<long> ReadedBookIds { get; set; }
        //public List<FrontBook> MyBooks { get; set; }

        public override User ToDbModel()
        {
            var user = new User
            {
                Id = Id,
                Name = Name,
                Password = Password,
                Email = Email,
                UserType = UserType != null ? (DalUserType)UserType.Value : DalUserType.Reader,
                AvatarUrl = AvatarUrl
            };

            return user;
        }
    }
}