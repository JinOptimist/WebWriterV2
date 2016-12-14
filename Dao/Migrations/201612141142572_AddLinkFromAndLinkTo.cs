namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLinkFromAndLinkTo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EventEvent", "Event_Id", "dbo.Event");
            DropForeignKey("dbo.EventEvent", "Event_Id1", "dbo.Event");
            DropIndex("dbo.EventEvent", new[] { "Event_Id" });
            DropIndex("dbo.EventEvent", new[] { "Event_Id1" });
            CreateTable(
                "dbo.EventLinkItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        From_Id = c.Long(),
                        To_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Event", t => t.From_Id)
                .ForeignKey("dbo.Event", t => t.To_Id)
                .Index(t => t.From_Id)
                .Index(t => t.To_Id);
            
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
            
            DropForeignKey("dbo.EventLinkItem", "To_Id", "dbo.Event");
            DropForeignKey("dbo.EventLinkItem", "From_Id", "dbo.Event");
            DropIndex("dbo.EventLinkItem", new[] { "To_Id" });
            DropIndex("dbo.EventLinkItem", new[] { "From_Id" });
            DropTable("dbo.EventLinkItem");
            CreateIndex("dbo.EventEvent", "Event_Id1");
            CreateIndex("dbo.EventEvent", "Event_Id");
            AddForeignKey("dbo.EventEvent", "Event_Id1", "dbo.Event", "Id");
            AddForeignKey("dbo.EventEvent", "Event_Id", "dbo.Event", "Id");
        }
    }
}
