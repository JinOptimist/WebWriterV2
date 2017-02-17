namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddDateTimeForHero : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Hero", "TimeCreation", c => c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"));
            AddColumn("dbo.Hero", "LastChanges", c => c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"));
        }

        public override void Down()
        {
            DropColumn("dbo.Hero", "LastChanges");
            DropColumn("dbo.Hero", "TimeCreation");
        }
    }
}
