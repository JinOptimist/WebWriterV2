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
            var UserFromVks = new List<UserFromVk>
            {
                new UserFromVk {FirstName = "Carson", Id = 1},
                new UserFromVk {FirstName = "Meredith", Id = 2},
                new UserFromVk {FirstName = "Arturo", Id = 3},
                new UserFromVk {FirstName = "Gytis", Id = 4},
                new UserFromVk {FirstName = "Yan", Id = 5},
                new UserFromVk {FirstName = "Peggy", Id = 6},
                new UserFromVk {FirstName = "Laura", Id = 7},
                new UserFromVk {FirstName = "Nino", Id = 8}
            };

            context.UserFromVk.AddRange(UserFromVks);
            context.SaveChanges();
        }
    }
}