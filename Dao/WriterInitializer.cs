using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Dal.Model;

namespace Dal
{
    public class WriterInitializer : DropCreateDatabaseIfModelChanges<WriterContext>
    {
        protected override void Seed(WriterContext context)
        {
            context.SaveChanges();
        }
    }
}