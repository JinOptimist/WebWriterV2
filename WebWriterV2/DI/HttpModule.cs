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
        public void Dispose()
        {
        }

        private void Application_BeginRequest(Object source, EventArgs e)
        {
            IUserRepository userRepository;
            using (var scope = StaticContainer.Container.BeginLifetimeScope()) {
                userRepository = scope.Resolve<IUserRepository>();
            }

            var userId = long.Parse(HttpContext.Current.Request?.Cookies["userId"]?.Value ?? "-1");
            var user = userId > 0 ? userRepository.Get(userId) : null;

            HttpContext.Current.User = new PrincipalUser(user);
        }

        public void Init(HttpApplication context)
        {
            context.PostAuthenticateRequest += (new EventHandler(Application_BeginRequest));
        }
    }
}