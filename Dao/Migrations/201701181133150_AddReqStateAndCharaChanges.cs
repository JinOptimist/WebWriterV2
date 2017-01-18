namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddReqStateAndCharaChanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventState1",
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

            AddColumn("dbo.State", "RequirementType", c => c.Int());
            AddColumn("dbo.Characteristic", "Event_Id", c => c.Long());
            CreateIndex("dbo.Characteristic", "Event_Id");
            AddForeignKey("dbo.Characteristic", "Event_Id", "dbo.Event", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.EventState1", "State_Id", "dbo.State");
            DropForeignKey("dbo.EventState1", "Event_Id", "dbo.Event");
            DropForeignKey("dbo.Characteristic", "Event_Id", "dbo.Event");
            DropIndex("dbo.EventState1", new[] { "State_Id" });
            DropIndex("dbo.EventState1", new[] { "Event_Id" });
            DropIndex("dbo.Characteristic", new[] { "Event_Id" });
            DropColumn("dbo.Characteristic", "Event_Id");
            DropColumn("dbo.State", "RequirementType");
            DropTable("dbo.EventState1");
        }
    }
}
