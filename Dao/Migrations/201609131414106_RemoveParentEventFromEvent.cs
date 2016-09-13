namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveParentEventFromEvent : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EventEvent", "Event_Id", "dbo.Event");
            DropForeignKey("dbo.EventEvent", "Event_Id1", "dbo.Event");
            DropIndex("dbo.EventEvent", new[] { "Event_Id" });
            DropIndex("dbo.EventEvent", new[] { "Event_Id1" });
            AddColumn("dbo.Event", "Event_Id", c => c.Long());
            CreateIndex("dbo.Event", "Event_Id");
            AddForeignKey("dbo.Event", "Event_Id", "dbo.Event", "Id");
            DropTable("dbo.EventEvent");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EventEvent",
                c => new
                    {
                        Event_Id = c.Long(nullable: false),
                        Event_Id1 = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Event_Id, t.Event_Id1 });
            
            DropForeignKey("dbo.Event", "Event_Id", "dbo.Event");
            DropIndex("dbo.Event", new[] { "Event_Id" });
            DropColumn("dbo.Event", "Event_Id");
            CreateIndex("dbo.EventEvent", "Event_Id1");
            CreateIndex("dbo.EventEvent", "Event_Id");
            AddForeignKey("dbo.EventEvent", "Event_Id1", "dbo.Event", "Id");
            AddForeignKey("dbo.EventEvent", "Event_Id", "dbo.Event", "Id");
        }
    }
}
