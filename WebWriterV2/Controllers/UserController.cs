using Dao;
using Dao.IRepository;
using Dao.Model;
using Dao.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using WebWriterV2.DI;
using WebWriterV2.FrontModels;
using WebWriterV2.RpgUtility;
using WebWriterV2.VkUtility;

namespace WebWriterV2.Controllers
{
    public class UserController : BaseApiController
    {
        public UserController(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        private IUserRepository UserRepository { get; }

        [AcceptVerbs("GET")]
        public FrontUser Login(string username, string password)
        {
            var user = UserRepository.Login(username, password);
            FrontUser frontUser = null;
            if (user != null) {
                frontUser = new FrontUser(user);
            }

            return frontUser;
        }

        [AcceptVerbs("GET")]
        public bool NameIsAvailable(string username)
        {
            var user = UserRepository.GetByName(username);
            return user == null;
        }

        [AcceptVerbs("POST")]
        public FrontUser Register(FrontUser frontUser)
        {
            var user = frontUser.ToDbModel();
            if (UserRepository.Exist(user)) {
                frontUser.Error = "Пользователь с таким именем или емалом уже существует";
                return frontUser;
            }
            
            user.ConfirmCode = RandomHelper.RandomString(RandomHelper.RandomInt(10, 20));
            user = UserRepository.Save(user);

            var relativeUrl = Url.Link("ConfirmRegister", new { userId = user.Id, confirmCode = user.ConfirmCode });
            EmailHelper.SendConfirmRegistrationEmail(relativeUrl, user.Email);

            frontUser = new FrontUser(user);
            return frontUser;
        }

        [AcceptVerbs("POST")]
        public object UploadAvatar(FakeModelString model)
        {
            // example of base64 string 
            // data:image/png;base64,iVBORw0KGg...
            var data = model.Data;
            var dataIndex = data.IndexOf("base64", StringComparison.Ordinal) + 7;
            var mark = "data:image/";
            var extensionStart = data.IndexOf(mark) + mark.Length;
            var extensionEnd = data.IndexOf(";");
            var extension = data.Substring(extensionStart, extensionEnd - extensionStart);

            var clearData = data.Substring(dataIndex);
            var fileData = Convert.FromBase64String(clearData);
            var bytes = fileData.ToArray();

            var path = PathHelper.PathToAvatar(User.Id, extension);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            using (var fileStream = System.IO.File.Create(path))
            {
                fileStream.Write(bytes, 0, bytes.Length);
                //TODO: investigate why we reload page on client after on server I close strea
                fileStream.Close();
            }

            User.AvatarUrl = PathHelper.PathToUrl(path);
            UserRepository.Save(User);

            return new { User.AvatarUrl };
        }

        [AcceptVerbs("GET")]
        public bool UpdateShowExtendedFunctionality(int userId, bool showExtendedFunctionality)
        {
            var user = UserRepository.Get(userId);
            user.ShowExtendedFunctionality = showExtendedFunctionality;
            UserRepository.Save(user);
            return showExtendedFunctionality;
        }
    }
}
