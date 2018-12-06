using Dal.IRepository;
using Dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebWriterV2.DI;

namespace WebWriterV2.Controllers
{
    public class BaseController : Controller
    {
        
        private User _user;
        /// <summary>
        /// Current user. Set on init request by Cookies["userId"].
        /// Can be null, if user not authorised
        /// </summary>
        protected new User User
        {
            get
            {
                if (_user == null)
                {
                    var container = StaticContainer.Container;
                    var userRepository = container.Resolve<IUserRepository>();
                    string userIdStr = HttpContext.Request.Cookies["userId"]?.Value;
                    var userId = long.Parse(string.IsNullOrEmpty(userIdStr) ? "-1" : userIdStr);
                    _user = userId > 0 ? userRepository.Get(userId) : null;
                }

                return _user;
            }
        }
    }
}