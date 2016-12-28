namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddThingToEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Thing", "Event_Id", c => c.Long());
            CreateIndex("dbo.Thing", "Event_Id");
            AddForeignKey("dbo.Thing", "Event_Id", "dbo.Event", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Thing", "Event_Id", "dbo.Event");
            DropIndex("dbo.Thing", new[] { "Event_Id" });
            DropColumn("dbo.Thing", "Event_Id");
        }
    }
}
