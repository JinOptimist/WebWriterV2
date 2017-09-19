using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Dao;
using Dao.Repository;
using System.Linq;
using WebWriterV2.DI;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Dao.IRepository;

namespace WebWriterV2
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        public static string Repository => "Repository";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var container = new WindsorContainer();

            //builder.RegisterModule();

            /* ************** Controller ************** */
            //container.RegisterControllers(typeof(MvcApplication).Assembly);

            /* ************** Repository ************** */
            var dalAssembly = typeof(BookRepository).Assembly;
            container.Register(Types.FromAssemblyContaining<BookRepository>()
                .Where(x => x.IsClass && x.Name.EndsWith(Repository))
                .WithService.AllInterfaces()
                .Configure(c => c.LifestylePerWebRequest()));
                
                //.Where(x => x.IsClass && x.Name.EndsWith(Repository))
                //.As(repositoryObject => repositoryObject.GetInterfaces().Single(x => x.Name.EndsWith(repositoryObject.Name)))
                //.InstancePerRequest();

            /* ************** WriterContext ************** */
            //var _writerContext = new WriterContext();
            container.Register(Component.For<WriterContext>().LifestylePerWebRequest());
                //<WriterContext>().As<WriterContext>().InstancePerRequest();

            

            /* ************** Store container in static ************** */

            //var container = container2.Build();
            //GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            StaticContainer.Container = container;

            WriterContext.SetInitializer();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Exception exception = Server.GetLastError();
            //Server.ClearError();
            //Response.Redirect("/Home/Error");
        }
    }
}
