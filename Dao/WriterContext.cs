using System.Data.Entity;
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

        public DbSet<Quest> Quest { get; set; }

        public DbSet<Event> Event { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Quest>().HasMany(u => u.AllEvents).WithRequired(x => x.Quest).WillCascadeOnDelete(false);

            modelBuilder.Entity<Quest>().HasOptional(x => x.RootEvent).WithOptionalPrincipal().WillCascadeOnDelete(false);

            modelBuilder.Entity<Event>().HasMany(u => u.ChildrenEvents).WithMany(x => x.ParentEvents);
            modelBuilder.Entity<Event>().HasMany(u => u.RequrmentSkill).WithMany();

            modelBuilder.Entity<Hero>().HasMany(u => u.Skills).WithMany();
            modelBuilder.Entity<Hero>().HasMany(u => u.State).WithMany();

            modelBuilder.Entity<Skill>().HasRequired(u => u.School).WithMany();
            modelBuilder.Entity<TrainingRoom>().HasRequired(u => u.School).WithMany();
        }

        public static void SetInitializer()
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<WriterContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WriterContext, Configuration>());
        }
    }
}