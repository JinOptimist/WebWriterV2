using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Dao;
using Dao.Repository;
using System.Linq;
using WebWriterV2.DI;

namespace WebWriterV2
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        public static string Repository => "Repository";

        private WriterContext _writerContext => new WriterContext();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var builder = new ContainerBuilder();

            /* ************** Controller ************** */
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            //TODO why this not work? :(
            //builder.RegisterApiControllers(typeof(MvcApplication).Assembly);
            // Or how I can run follow code
            //builder.Register(cont => new BookController(cont.Resolve<IBookRepository>()));

            /* ************** Repository ************** */
            var dalAssembly = typeof(BookRepository).Assembly;
            builder.RegisterAssemblyTypes(dalAssembly)
                .Where(x => x.IsClass && x.Name.EndsWith(Repository))
                .As(repositoryObject => repositoryObject.GetInterfaces().Single(x => x.Name.EndsWith(repositoryObject.Name)));

            /* ************** WriterContext ************** */
            builder.RegisterInstance(_writerContext).As<WriterContext>();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            StaticContainer.Container = container;

            WriterContext.SetInitializer();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var a = 1;
            //Exception exception = Server.GetLastError();
            //Server.ClearError();
            //Response.Redirect("/Home/Error");
        }
    }
}
