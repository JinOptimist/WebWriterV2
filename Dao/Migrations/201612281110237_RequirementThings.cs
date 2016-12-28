namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class RequirementThings : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Thing", "Event_Id", "dbo.Event");
            AddColumn("dbo.Thing", "Event_Id1", c => c.Long());
            CreateIndex("dbo.Thing", "Event_Id1");
            AddForeignKey("dbo.Thing", "Event_Id", "dbo.Event", "Id");
            AddForeignKey("dbo.Thing", "Event_Id1", "dbo.Event", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Thing", "Event_Id1", "dbo.Event");
            DropForeignKey("dbo.Thing", "Event_Id", "dbo.Event");
            DropIndex("dbo.Thing", new[] { "Event_Id1" });
            DropColumn("dbo.Thing", "Event_Id1");
            AddForeignKey("dbo.Thing", "Event_Id", "dbo.Event", "Id");
        }
    }
}
