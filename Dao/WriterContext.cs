using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Conventions;
using Dao.Migrations;
using Dao.Model;

namespace Dao
{
    public class WriterContext : DbContext
    {
        public WriterContext() : base("name=WriterContext")
        {
            //Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<User>().HasMany(x => x.Books).WithOptional(x => x.Owner);
            modelBuilder.Entity<User>().HasMany(x => x.Bookmarks).WithOptional(x => x.Owner);
            modelBuilder.Entity<User>().HasMany(x => x.StateTypes).WithOptional(x => x.Owner);
            modelBuilder.Entity<User>().HasMany(x => x.ThingsSample).WithOptional(x => x.Owner);
            modelBuilder.Entity<User>().HasMany(x => x.BooksAreReaded).WithMany();
            modelBuilder.Entity<User>().HasMany(x => x.Evaluations).WithOptional(x => x.Owner);

            modelBuilder.Entity<Book>().HasOptional(x => x.RootEvent).WithOptionalPrincipal(x => x.ForRootBook).WillCascadeOnDelete(false);
            modelBuilder.Entity<Book>().HasMany(u => u.AllEvents).WithRequired(x => x.Book).WillCascadeOnDelete(false);
            modelBuilder.Entity<Book>().HasMany(u => u.Evaluations).WithOptional(x => x.Book);

            modelBuilder.Entity<Event>().HasMany(x => x.ThingsChanges).WithOptional();
            modelBuilder.Entity<Event>().HasMany(x => x.RequirementThings).WithOptional();
            modelBuilder.Entity<Event>().HasMany(x => x.RequirementStates).WithMany();
            modelBuilder.Entity<Event>().HasMany(x => x.HeroStatesChanging).WithMany();

            modelBuilder.Entity<EventLinkItem>().HasOptional(x => x.To).WithMany(x => x.LinksToThisEvent);
            modelBuilder.Entity<EventLinkItem>().HasOptional(x => x.From).WithMany(x => x.LinksFromThisEvent);

            modelBuilder.Entity<Genre>().HasMany(u => u.Books).WithOptional(x => x.Genre);

            modelBuilder.Entity<Hero>().HasMany(u => u.State).WithOptional().WillCascadeOnDelete(true);
            modelBuilder.Entity<Hero>().HasMany(u => u.Inventory).WithOptional(x => x.Hero);
            //modelBuilder.Entity<Hero>().HasOptional(u => u.CurrentEvent)

            //TODO Manual remove state for SelfChanging
            //.WillCascadeOnDelete(true);
            modelBuilder.Entity<ThingSample>().HasMany(u => u.PassiveStates).WithOptional();
            modelBuilder.Entity<ThingSample>().HasMany(u => u.UsingEffectState).WithOptional();

            modelBuilder.Entity<Thing>().HasOptional(u => u.ThingSample).WithMany();
            modelBuilder.Entity<Thing>().HasOptional(u => u.Hero).WithMany(x => x.Inventory);

            modelBuilder.Entity<State>().HasOptional(u => u.StateType).WithMany();
        }

        public static void SetInitializer()
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<WriterContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WriterContext, Configuration>());
        }
    }
}