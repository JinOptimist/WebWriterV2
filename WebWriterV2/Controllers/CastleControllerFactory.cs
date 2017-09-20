using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebWriterV2.Controllers
{
    public class CastleControllerFactory : DefaultControllerFactory
    {
        //контейнер
        public IWindsorContainer Container { get; protected set; }

        public CastleControllerFactory(IWindsorContainer container)
        {
            if (container == null) {
                throw new ArgumentNullException("container");
            }

            Container = container;
        }

        //получение контроллера для обработки запроса
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null) {
                return null;
            }
            // получаем запрошенный контроллер от Castle
            return Container.Resolve(controllerType) as IController;
        }

        // освобождаем контроллер
        public override void ReleaseController(IController controller)
        {
            var disposableController = controller as IDisposable;
            if (disposableController != null) {
                disposableController.Dispose();
            }

            // информируем ioc-контейнер, что контроллер нам больше не нужен
            Container.Release(controller);
        }
    }
}