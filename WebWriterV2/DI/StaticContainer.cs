using Castle.Windsor;
using Dao;

namespace WebWriterV2.DI
{
    public static class StaticContainer
    {
        public static IWindsorContainer Container { get; set; }
    }
}