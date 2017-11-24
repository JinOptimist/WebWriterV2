namespace Dao.Migrations
{
    using Dao.DataGeneration;
    using Dao.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<Dao.WriterContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

            ContextKey = "Dao.WriterContext";
        }

        protected override void Seed(Dao.WriterContext context)
        {
            //  This method will be called after migrating to the latest version.
            var admin = GenerateData.GenerateAdmin();
            context.Users.AddOrUpdate(x => x.Id, admin);
            admin = context.Users.Single(x => x.Id == admin.Id);

            var stateTypes = GenerateData.GenerateStateTypes();
            stateTypes.ForEach(x => x.Owner = admin);
            context.StateTypes.AddOrUpdate(x => x.Name, stateTypes.ToArray());

            var genres = GenerateData.GenerateGenres();
            context.Genres.AddOrUpdate(x => x.Name, genres.ToArray());

            //var book = GenerateData.BookRat();
            //book.Owner = admin;
            ////var tower = GenerateData.BookTower(stateTypes);
            //context.Books.AddOrUpdate(x => x.Name, book);

        }
    }
}
