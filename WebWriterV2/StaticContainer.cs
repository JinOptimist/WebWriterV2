using Autofac;
using Dao;

namespace WebWriterV2
{
    public static class StaticContainer
    {
        private static WriterContext _context;

        public static IContainer Container { get; set; }

        public static WriterContext Context => _context ?? (_context = new WriterContext());
    }
}