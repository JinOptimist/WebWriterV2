using Dao.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebWriterV2.DI;

namespace WebWriterV2.Controllers
{
    public class MyController : Controller
    {
        /// <summary>
        /// Current user. Set on init request by Cookies["userId"].
        /// Can be null, if user not authorised
        /// </summary>
        protected new User User => (HttpContext.User as PrincipalUser).User;
    }
}