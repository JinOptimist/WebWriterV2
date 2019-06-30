using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Conventions;
using Dal.Migrations;
using Dal.Model;

namespace Dal
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
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Article> Articles { get; set; }

        public virtual DbSet<Questionnaire> Questionnaires { get; set; }
        public virtual DbSet<QuestionnaireResult> QuestionnaireResults { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }

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
            modelBuilder.Entity<User>().HasMany(x => x.Questionnaires).WithMany(x => x.Users);
            modelBuilder.Entity<User>().HasMany(x => x.QuestionnaireResults).WithOptional(x => x.User);
            modelBuilder.Entity<User>().HasMany(x => x.Likes).WithRequired(x => x.User).WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>().HasMany(x => x.AllChapters).WithRequired(x => x.Book);
            modelBuilder.Entity<Book>().HasMany(x => x.Travels).WithOptional(x => x.Book);
            modelBuilder.Entity<Book>().HasOptional(x => x.RootChapter).WithOptionalPrincipal(x => x.ForRootBook);
            modelBuilder.Entity<Book>().HasMany(x => x.States).WithOptional(x => x.Book);
            modelBuilder.Entity<Book>().HasMany(x => x.Evaluations).WithRequired(x => x.Book).WillCascadeOnDelete(false);
            modelBuilder.Entity<Book>().HasOptional(x => x.Genre).WithMany(x => x.Books);
            modelBuilder.Entity<Book>().HasMany(x => x.Readers).WithRequired(x => x.Book);
            modelBuilder.Entity<Book>().HasMany(x => x.Tags).WithMany(x => x.Books);
            modelBuilder.Entity<Book>().HasMany(x => x.CoAuthors).WithMany(x => x.AvailableButNotMineBooks);
            modelBuilder.Entity<Book>().HasMany(x => x.Likes).WithRequired(x => x.Book).WillCascadeOnDelete(false);

            modelBuilder.Entity<Chapter>().HasMany(x => x.LinksFromThisChapter).WithRequired(x => x.From).WillCascadeOnDelete(false);
            modelBuilder.Entity<Chapter>().HasMany(x => x.LinksToThisChapter).WithRequired(x => x.To);

            modelBuilder.Entity<ChapterLinkItem>().HasMany(x => x.StateRequirement).WithOptional(x => x.ChapterLink);
            modelBuilder.Entity<ChapterLinkItem>().HasMany(x => x.StateChanging).WithOptional(x => x.ChapterLink);
            modelBuilder.Entity<ChapterLinkItem>().HasMany(x => x.TravelSteps).WithOptional(x => x.Choice).WillCascadeOnDelete(true);

            modelBuilder.Entity<Travel>().HasMany(x => x.Steps).WithRequired(x => x.Travel);
            modelBuilder.Entity<Travel>().HasOptional(x => x.CurrentStep).WithOptionalDependent(x => x.TravelForCurrentStep);

            modelBuilder.Entity<TravelStep>().HasOptional(x => x.PrevStep).WithOptionalDependent(x => x.NextStep);

            modelBuilder.Entity<StateType>().HasMany(x => x.Changes).WithRequired(x => x.StateType);
            modelBuilder.Entity<StateType>().HasMany(x => x.Requirements).WithRequired(x => x.StateType);
            modelBuilder.Entity<StateType>().HasMany(x => x.Values).WithRequired(x => x.StateType);

            modelBuilder.Entity<Tag>().HasMany(x => x.Books).WithMany(x => x.Tags);

            modelBuilder.Entity<Questionnaire>().HasMany(x => x.Questions).WithRequired(x => x.Questionnaire);
            modelBuilder.Entity<Questionnaire>().HasMany(x => x.QuestionnaireResults).WithRequired(x => x.Questionnaire);
            modelBuilder.Entity<Questionnaire>().HasMany(x => x.Users).WithMany(x => x.Questionnaires);

            modelBuilder.Entity<Question>().HasMany(x => x.OtherAnswers).WithOptional(x => x.Question); 
            modelBuilder.Entity<Question>().HasMany(x => x.Answers).WithOptional(x => x.Question);
            modelBuilder.Entity<Question>().HasMany(x => x.VisibleIf).WithMany(x => x.AffectVisibilityOfQuestions);

            modelBuilder.Entity<QuestionnaireResult>().HasMany(x => x.QuestionOtherAnswers).WithRequired(x => x.QuestionnaireResult);
            modelBuilder.Entity<QuestionnaireResult>().HasMany(x => x.QuestionAnswers).WithMany(x => x.QuestionnaireResults);
        }

        public static void SetInitializer()
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<WriterContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WriterContext, Configuration>());
        }
    }
}