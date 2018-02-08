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

        /* Main entities */
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Chapter> Chapters { get; set; }
        public virtual DbSet<ChapterLinkItem> ChapterLinkItems { get; set; }
        /* State entities */
        public virtual DbSet<StateChange> StateChanges { get; set; }
        public virtual DbSet<StateRequirement> StateRequirements { get; set; }
        public virtual DbSet<StateValue> StateValues { get; set; }
        public virtual DbSet<StateType> StateTypes { get; set; }
        /* Custom entities */
        public virtual DbSet<Travel> Travels { get; set; }
        public virtual DbSet<TravelStep> TravelSteps { get; set; }
        public virtual DbSet<Evaluation> Evaluations { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<User>().HasMany(x => x.Books).WithRequired(x => x.Owner);
            modelBuilder.Entity<User>().HasMany(x => x.BooksAreReaded).WithOptional(x => x.User).WillCascadeOnDelete(false);
            modelBuilder.Entity<User>().HasMany(x => x.StateTypes).WithRequired(x => x.Owner);
            //modelBuilder.Entity<User>().HasMany(x => x.Bookmarks).WithRequired(x => x.Owner);
            modelBuilder.Entity<User>().HasMany(x => x.MyTravels).WithOptional(x => x.Reader);
            modelBuilder.Entity<User>().HasMany(x => x.Evaluations).WithRequired(x => x.Owner).WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>().HasMany(x => x.AllChapters).WithRequired(x => x.Book);
            modelBuilder.Entity<Book>().HasMany(x => x.Travels).WithOptional(x => x.Book);
            modelBuilder.Entity<Book>().HasOptional(x => x.RootChapter).WithOptionalPrincipal(x => x.ForRootBook);
            modelBuilder.Entity<Book>().HasMany(x => x.Evaluations).WithRequired(x => x.Book).WillCascadeOnDelete(false);
            modelBuilder.Entity<Book>().HasOptional(x => x.Genre).WithMany(x => x.Books);
            modelBuilder.Entity<Book>().HasMany(x => x.Readers).WithRequired(x => x.Book);

            modelBuilder.Entity<Chapter>().HasMany(x => x.LinksFromThisChapter).WithRequired(x => x.From).WillCascadeOnDelete(false);
            modelBuilder.Entity<Chapter>().HasMany(x => x.LinksToThisChapter).WithRequired(x => x.To);

            modelBuilder.Entity<ChapterLinkItem>().HasMany(x => x.RequirementStates).WithOptional(x => x.Chapter);
            modelBuilder.Entity<ChapterLinkItem>().HasMany(x => x.HeroStatesChanging).WithOptional(x => x.Chapter);
            modelBuilder.Entity<ChapterLinkItem>().HasMany(x => x.TravelSteps).WithOptional(x => x.Сhoice);

            modelBuilder.Entity<Travel>().HasMany(x => x.Steps).WithRequired(x => x.Travel);

            modelBuilder.Entity<StateType>().HasMany(x => x.Changes).WithRequired(x => x.StateType);
            modelBuilder.Entity<StateType>().HasMany(x => x.Requirements).WithRequired(x => x.StateType);
            modelBuilder.Entity<StateType>().HasMany(x => x.Values).WithRequired(x => x.StateType);

            //.WillCascadeOnDelete(true);
        }

        public static void SetInitializer()
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<WriterContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WriterContext, Configuration>());
        }
    }
}