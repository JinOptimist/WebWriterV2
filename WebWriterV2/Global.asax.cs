using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Dao;
using Dao.IRepository;
using Dao.Repository;
using Newtonsoft.Json;

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

            var context = new WriterContext();
            var aaa = new TypedParameter(typeof(WriterContext), context);
            var builder = new ContainerBuilder();
            /* ************** RPG ************** */

            builder.RegisterType<QuestRepository>()
                .As<IQuestRepository>()
                .WithParameter(new TypedParameter(typeof(WriterContext), context));

            builder.RegisterType<EventRepository>()
                .As<IEventRepository>()
                .WithParameter(new TypedParameter(typeof(WriterContext), context));


            //builder.RegisterType<QuestRepository>();
            //builder.RegisterType<EventRepository>();
            //builder.Register<IQuestRepository>(x => x.Resolve<QuestRepository>());
            //builder.Register<IEventRepository>(x => x.Resolve<EventRepository>());


            StaticContainer.Container = builder.Build();

            WriterContext.SetInitializer();

            //var mark = Mark.Instance;
            //mark.Start();
        }
    }
}
