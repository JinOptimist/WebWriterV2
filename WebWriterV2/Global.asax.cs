﻿using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Dao;
using Dao.IRepository;
using Dao.Migrations;
using Dao.Repository;
using WebWriterV2.SecondThread;

namespace WebWriterV2
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            var builder = new ContainerBuilder();
            builder.RegisterType<UserRepository>();
            builder.RegisterType<HeroRepository>();
            builder.RegisterType<FriendIdRepository>();
            builder.Register<IUserRepository>(x => x.Resolve<UserRepository>());
            builder.Register<IHeroRepository>(x => x.Resolve<HeroRepository>());
            builder.Register<IFriendIdRepository>(x => x.Resolve<FriendIdRepository>());
            
            StaticContainer.Container = builder.Build();

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WriterContext, Configuration>());

            //var mark = Mark.Instance;
            //mark.Start();
        }
    }
}