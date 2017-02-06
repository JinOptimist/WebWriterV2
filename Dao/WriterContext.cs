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

        //public DbSet<Quest> Quest { get; set; }
        //public DbSet<Event> Event { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<User>().HasMany(x => x.Quests).WithOptional(x => x.Owner);
            modelBuilder.Entity<User>().HasMany(x => x.Bookmarks).WithOptional(x => x.Owner);

            modelBuilder.Entity<Quest>().HasOptional(x => x.RootEvent).WithOptionalPrincipal(x => x.ForRootQuest).WillCascadeOnDelete(false);
            modelBuilder.Entity<Quest>().HasMany(u => u.AllEvents).WithRequired(x => x.Quest).WillCascadeOnDelete(false);

            modelBuilder.Entity<Event>().HasMany(x => x.ThingsChanges).WithOptional();
            modelBuilder.Entity<Event>().HasMany(x => x.CharacteristicsChanges).WithOptional();
            modelBuilder.Entity<Event>().HasMany(x => x.RequirementThings).WithOptional();
            modelBuilder.Entity<Event>().HasMany(x => x.RequirementSkill).WithMany();
            modelBuilder.Entity<Event>().HasMany(x => x.RequirementStates).WithMany();
            modelBuilder.Entity<Event>().HasMany(x => x.RequirementCharacteristics).WithMany();
            modelBuilder.Entity<Event>().HasMany(x => x.HeroStatesChanging).WithMany();

            modelBuilder.Entity<EventLinkItem>().HasOptional(x => x.To).WithMany(x => x.LinksToThisEvent);
            modelBuilder.Entity<EventLinkItem>().HasOptional(x => x.From).WithMany(x => x.LinksFromThisEvent);

            modelBuilder.Entity<Hero>().HasMany(u => u.Skills).WithMany();
            modelBuilder.Entity<Hero>().HasMany(u => u.State).WithOptional().WillCascadeOnDelete(true);
            modelBuilder.Entity<Hero>().HasMany(u => u.Characteristics).WithOptional().WillCascadeOnDelete(true);
            modelBuilder.Entity<Hero>().HasMany(u => u.Inventory).WithOptional(x => x.Hero);
            //modelBuilder.Entity<Hero>().HasOptional(u => u.CurrentEvent)

            //TODO Manual remove state for SelfChanging
            //.WillCascadeOnDelete(true);
            modelBuilder.Entity<Skill>().HasMany(u => u.SelfChanging).WithOptional();
            modelBuilder.Entity<Skill>().HasMany(u => u.TargetChanging).WithOptional().WillCascadeOnDelete(true);
            modelBuilder.Entity<Skill>().HasOptional(u => u.School).WithMany(x => x.Skills);

            modelBuilder.Entity<SkillSchool>().HasMany(u => u.Skills).WithOptional(x => x.School);

            modelBuilder.Entity<TrainingRoom>().HasOptional(u => u.School).WithMany();

            modelBuilder.Entity<Guild>().HasMany(u => u.Heroes).WithOptional(x => x.Guild).WillCascadeOnDelete(true);

            modelBuilder.Entity<ThingSample>().HasMany(u => u.PassiveCharacteristics).WithOptional();
            modelBuilder.Entity<ThingSample>().HasMany(u => u.PassiveStates).WithOptional();
            modelBuilder.Entity<ThingSample>().HasMany(u => u.UsingEffectCharacteristics).WithOptional();
            modelBuilder.Entity<ThingSample>().HasMany(u => u.UsingEffectState).WithOptional();

            modelBuilder.Entity<Thing>().HasOptional(u => u.ThingSample).WithMany();
            modelBuilder.Entity<Thing>().HasOptional(u => u.Hero).WithMany(x => x.Inventory);

            modelBuilder.Entity<CharacteristicType>().HasMany(u => u.EffectState).WithOptional().WillCascadeOnDelete(true);

            modelBuilder.Entity<Characteristic>().HasOptional(u => u.CharacteristicType).WithMany();

            modelBuilder.Entity<State>().HasOptional(u => u.StateType).WithMany();
        }

        public static void SetInitializer()
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<WriterContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WriterContext, Configuration>());
        }
    }
}