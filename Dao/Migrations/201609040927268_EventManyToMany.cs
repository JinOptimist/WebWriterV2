namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventManyToMany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Event", "Event_Id", "dbo.Event");
            DropIndex("dbo.Event", new[] { "Event_Id" });
            CreateTable(
                "dbo.EventEvent",
                c => new
                    {
                        Event_Id = c.Long(nullable: false),
                        Event_Id1 = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Event_Id, t.Event_Id1 })
                .ForeignKey("dbo.Event", t => t.Event_Id)
                .ForeignKey("dbo.Event", t => t.Event_Id1)
                .Index(t => t.Event_Id)
                .Index(t => t.Event_Id1);
            
            DropColumn("dbo.Event", "Event_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Event", "Event_Id", c => c.Long());
            DropForeignKey("dbo.EventEvent", "Event_Id1", "dbo.Event");
            DropForeignKey("dbo.EventEvent", "Event_Id", "dbo.Event");
            DropIndex("dbo.EventEvent", new[] { "Event_Id1" });
            DropIndex("dbo.EventEvent", new[] { "Event_Id" });
            DropTable("dbo.EventEvent");
            CreateIndex("dbo.Event", "Event_Id");
            AddForeignKey("dbo.Event", "Event_Id", "dbo.Event", "Id");
        }
    }
}
