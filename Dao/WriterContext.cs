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

        public DbSet<UserFromVk> UserFromVk { get; set; }

        public DbSet<FriendId> FriendId { get; set; }

        public DbSet<Hero> Hero { get; set; }

        public DbSet<StudentLogin> StudentLogin { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<UserFromVk>().HasMany(u => u.FriendIds).WithRequired(t => t.UserFromVk);

            //modelBuilder.Entity<Course>()
            //    .HasMany(c => c.Instructors).WithMany(i => i.Courses)
            //    .Map(t => t.MapLeftKey("CourseID")
            //        .MapRightKey("InstructorID")
            //        .ToTable("CourseInstructor"));
            //modelBuilder.Entity<Department>().MapToStoredProcedures();
        }

        public static void SetInitializer()
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<WriterContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WriterContext, Configuration>());
        }
    }
}