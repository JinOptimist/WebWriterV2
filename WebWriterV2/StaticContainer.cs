using Autofac;
using Dao;

namespace WebWriterV2
{
    public static class StaticContainer
    {
        public static IContainer Container { get; set; }

        private static WriterContext _context;
        public static WriterContext Context => _context ?? (_context = new WriterContext());
    }
}