namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class EventHasChara : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventState",
                c => new
                    {
                        Event_Id = c.Long(nullable: false),
                        State_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Event_Id, t.State_Id })
                .ForeignKey("dbo.Event", t => t.Event_Id, cascadeDelete: true)
                .ForeignKey("dbo.State", t => t.State_Id, cascadeDelete: true)
                .Index(t => t.Event_Id)
                .Index(t => t.State_Id);

            CreateTable(
                "dbo.EventCharacteristic",
                c => new
                    {
                        Event_Id = c.Long(nullable: false),
                        Characteristic_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Event_Id, t.Characteristic_Id })
                .ForeignKey("dbo.Event", t => t.Event_Id, cascadeDelete: true)
                .ForeignKey("dbo.Characteristic", t => t.Characteristic_Id, cascadeDelete: true)
                .Index(t => t.Event_Id)
                .Index(t => t.Characteristic_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.EventCharacteristic", "Characteristic_Id", "dbo.Characteristic");
            DropForeignKey("dbo.EventCharacteristic", "Event_Id", "dbo.Event");
            DropForeignKey("dbo.EventState", "State_Id", "dbo.State");
            DropForeignKey("dbo.EventState", "Event_Id", "dbo.Event");
            DropIndex("dbo.EventCharacteristic", new[] { "Characteristic_Id" });
            DropIndex("dbo.EventCharacteristic", new[] { "Event_Id" });
            DropIndex("dbo.EventState", new[] { "State_Id" });
            DropIndex("dbo.EventState", new[] { "Event_Id" });
            DropTable("dbo.EventCharacteristic");
            DropTable("dbo.EventState");
        }
    }
}
