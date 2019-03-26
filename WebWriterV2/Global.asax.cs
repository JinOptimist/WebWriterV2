using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Dal;
using Dal.Repository;
using System.Linq;
using WebWriterV2.DI;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Dal.IRepository;
using WebWriterV2.Controllers;
using NLog;

namespace WebWriterV2
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static string Repository => "Repository";

        protected void Application_Start()
        {
            logger.Info("Application start");
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var container = new WindsorContainer();

            //builder.RegisterModule();

            /* ************** Controller ************** */
            container.Register(Classes.FromThisAssembly().BasedOn<BaseApiController>().LifestyleTransient());
            container.Register(Classes.FromThisAssembly().BasedOn<BaseController>().LifestyleTransient());
            
            /* ************** Repository ************** */
            var dalAssembly = typeof(BookRepository).Assembly;
            container.Register(Types.FromAssemblyContaining<BookRepository>()
                .Where(x => x.IsClass && x.Name.EndsWith(Repository))
                .WithService.AllInterfaces()
                .Configure(c => c.LifestylePerWebRequest()));

            /* ************** WriterContext ************** */
            container.Register(Component.For<WriterContext>().LifestylePerWebRequest());

            /* ************** Store container in static ************** */
            GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(container);
            ControllerBuilder.Current.SetControllerFactory(new CastleControllerFactory(container));
            StaticContainer.Container = container;
            WriterContext.SetInitializer();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            logger.Error(exception, "Application_Error");

            Server.ClearError();
            if (exception.Source == "System.Web.Mvc") {
                RpgUtility.EmailHelper.SendUnexpectedRequest(exception, Request);
            } else {
                RpgUtility.EmailHelper.SendError(exception);
            }
            
            //Response.Redirect("/Home/Error");
        }
    }
}
