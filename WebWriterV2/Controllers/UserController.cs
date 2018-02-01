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
        public UserController(IBookRepository bookRepository, IEvaluationRepository evaluationRepository, IChapterLinkItemRepository eventLinkItemRepository, IChapterRepository eventRepository, IStateValueRepository stateRepository, IGenreRepository genreRepository, IUserRepository userRepository)
        {
            BookRepository = bookRepository;
            EvaluationRepository = evaluationRepository;
            EventLinkItemRepository = eventLinkItemRepository;
            EventRepository = eventRepository;
            StateRepository = stateRepository;
            GenreRepository = genreRepository;
            UserRepository = userRepository;
        }

        private IBookRepository BookRepository { get; }
        private IEvaluationRepository EvaluationRepository { get; }
        private IChapterLinkItemRepository EventLinkItemRepository { get; }
        private IChapterRepository EventRepository { get; }
        private IStateValueRepository StateRepository { get; }
        private IGenreRepository GenreRepository { get; }
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
            user.ConfirmCode = RandomHelper.RandomString(RandomHelper.RandomInt(10, 20));
            user = UserRepository.Save(user);
            frontUser = new FrontUser(user);

            var relativeUrl = Url.Link("ConfirmRegister", new { userId = user.Id, confirmCode = user.ConfirmCode });
            var url = EmailHelper.ToAbsoluteUrl(relativeUrl);
            var body = $"Пожалуйста подтвердите регистрацию. Для этого достаточно перейти по ссылке {url}";
            var title = "Интерактивная книга. Регистрация";
            EmailHelper.Send(user.Email, title, body);

            return frontUser;
        }

        [AcceptVerbs("POST")]
        public object UploadAvatar(FakeModelString model)
        {
            //example of base64 string 
            //data:image/png;base64,iVBORw0KGg...
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

    }
}
