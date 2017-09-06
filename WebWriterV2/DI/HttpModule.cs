using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dao.Model;
using Dao.IRepository;
using Autofac;
using System.Security.Principal;

namespace WebWriterV2.DI
{
    public class HttpModule : IHttpModule
    {
        /// <summary>
        /// Current user. Set on init request by Cookies["userId"].
        /// Can be null, if user not authorised
        /// </summary>
        public static User User { get { return (HttpContext.Current.User as PrincipalUser).User; } }

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            IUserRepository userRepository;
            using (var scope = StaticContainer.Container.BeginLifetimeScope()) {
                userRepository = scope.Resolve<IUserRepository>();
            }

            var userId = long.Parse(HttpContext.Current.Request.Cookies["userId"]?.Value ?? "-1");
            var user = userId > 0 ? userRepository.Get(userId) : null;

            HttpContext.Current.User = new PrincipalUser(user);
            
        }
    }
}