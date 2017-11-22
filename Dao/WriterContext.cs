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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<User>().HasMany(x => x.Books).WithRequired(x => x.Owner);
            modelBuilder.Entity<User>().HasMany(x => x.BooksAreReaded).WithOptional(x => x.User).WillCascadeOnDelete(false);
            modelBuilder.Entity<User>().HasMany(x => x.StateTypes).WithRequired(x => x.Owner);
            modelBuilder.Entity<User>().HasMany(x => x.Bookmarks).WithRequired(x => x.Owner);
            modelBuilder.Entity<User>().HasMany(x => x.Evaluations).WithRequired(x => x.Owner).WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>().HasMany(x => x.AllChapters).WithRequired(x => x.Book);
            modelBuilder.Entity<Book>().HasRequired(x => x.RootChapter).WithRequiredPrincipal(x => x.ForRootBook);
            modelBuilder.Entity<Book>().HasMany(x => x.Evaluations).WithRequired(x => x.Book).WillCascadeOnDelete(false);
            modelBuilder.Entity<Book>().HasOptional(x => x.Genre).WithMany(x => x.Books);
            modelBuilder.Entity<Book>().HasMany(x => x.Readers).WithRequired(x => x.Book);

            modelBuilder.Entity<Chapter>().HasMany(x => x.LinksFromThisChapter).WithRequired(x => x.From);
            modelBuilder.Entity<Chapter>().HasMany(x => x.LinksToThisChapter).WithRequired(x => x.To);

            modelBuilder.Entity<ChapterLinkItem>().HasMany(x => x.RequirementStates).WithOptional(x => x.Chapter);
            modelBuilder.Entity<ChapterLinkItem>().HasMany(x => x.HeroStatesChanging).WithOptional(x => x.Chapter);

            modelBuilder.Entity<StateChange>().HasRequired(x => x.StateType).WithMany();
            modelBuilder.Entity<StateRequirement>().HasRequired(x => x.StateType).WithMany();

            //TODO Manual remove state for SelfChanging
            //.WillCascadeOnDelete(true);
            //modelBuilder.Entity<ThingSample>().HasMany(u => u.PassiveStates).WithOptional();
            //modelBuilder.Entity<ThingSample>().HasMany(u => u.UsingEffectState).WithOptional();

            //modelBuilder.Entity<Thing>().HasOptional(u => u.ThingSample).WithMany();
            //modelBuilder.Entity<Thing>().HasOptional(u => u.Hero).WithMany(x => x.Inventory);

            //modelBuilder.Entity<StateValue>().HasOptional(u => u.StateType).WithMany();
        }

        public static void SetInitializer()
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<WriterContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WriterContext, Configuration>());
        }
    }
}