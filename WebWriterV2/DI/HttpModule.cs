using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dao.Model;
using Dao.IRepository;
using System.Security.Principal;
using Dao;
using Dao.Repository;

namespace WebWriterV2.DI
{
    public class HttpModule : IHttpModule
    {
        private void Application_BeginRequest(Object source, EventArgs e)
        {
            //var writerContext = new WriterContext();
            //var userRepository = new UserRepository(writerContext);

            //var userId = long.Parse(HttpContext.Current.Request?.Cookies["userId"]?.Value ?? "-1");
            //var user = userId > 0 ? userRepository.Get(userId) : null;

            //HttpContext.Current.User = new PrincipalUser(user);
        }

        //private void Application_EndRequest(Object source, EventArgs e)
        //{
        //    StaticContainer.Container.Dispose();
        //}

        public void Init(HttpApplication context)
        {
            context.PostAuthenticateRequest += (new EventHandler(Application_BeginRequest));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}