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

        //public DbSet<Quest> Quest { get; set; }

        //public DbSet<Event> Event { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Quest>().HasOptional(x => x.RootEvent).WithOptionalPrincipal(x => x.ForRootQuest).WillCascadeOnDelete(false);
            modelBuilder.Entity<Quest>().HasMany(u => u.AllEvents).WithRequired(x => x.Quest).WillCascadeOnDelete(false);

            modelBuilder.Entity<Event>().HasMany(u => u.ChildrenEvents).WithMany(x => x.ParentEvents);
            modelBuilder.Entity<Event>().HasMany(u => u.RequrmentSkill).WithMany();

            modelBuilder.Entity<Hero>().HasMany(u => u.Skills).WithMany();
            modelBuilder.Entity<Hero>().HasMany(u => u.State).WithOptional().WillCascadeOnDelete(true);

            modelBuilder.Entity<Skill>().HasOptional(u => u.School).WithMany();

            //TODO Manual remove state for SelfChanging
            //.WillCascadeOnDelete(true);
            modelBuilder.Entity<Skill>().HasMany(u => u.SelfChanging).WithOptional();
            modelBuilder.Entity<Skill>().HasMany(u => u.TargetChanging).WithOptional().WillCascadeOnDelete(true);

            modelBuilder.Entity<TrainingRoom>().HasOptional(u => u.School).WithMany();

            modelBuilder.Entity<Guild>().HasMany(u => u.Heroes).WithOptional(x => x.Guild).WillCascadeOnDelete(true);

            modelBuilder.Entity<Thing>().HasMany(u => u.OnceEffect).WithOptional().WillCascadeOnDelete(true);
            modelBuilder.Entity<Thing>().HasMany(u => u.Changing).WithOptional().WillCascadeOnDelete(true);

            modelBuilder.Entity<Characteristic>().HasOptional(u => u.CharacteristicType).WithMany();

            modelBuilder.Entity<CharacteristicType>().HasMany(u => u.EffectState).WithOptional().WillCascadeOnDelete(true);

            modelBuilder.Entity<State>().HasOptional(u => u.StateType).WithMany();
        }

        public static void SetInitializer()
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<WriterContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WriterContext, Configuration>());
        }
    }
}