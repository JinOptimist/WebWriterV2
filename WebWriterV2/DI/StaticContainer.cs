using Castle.Windsor;
using Dal;

namespace WebWriterV2.DI
{
    public static class StaticContainer
    {
        private static IWindsorContainer _container;
        public static IWindsorContainer Container
        {
            get
            {
                return _container;
            }

            set
            {
                if (value == null)
                    throw new System.Exception("Container can't be null");
                if (_container != null)
                    throw new System.Exception("You mustn't rewrite Container! Container must be initialized only once");
                _container = value;
            }
        }
    }
}