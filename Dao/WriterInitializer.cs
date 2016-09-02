using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Dao.Model;

namespace Dao
{
    public class WriterInitializer : DropCreateDatabaseIfModelChanges<WriterContext>
    {
        protected override void Seed(WriterContext context)
        {
            context.SaveChanges();
        }
    }
}