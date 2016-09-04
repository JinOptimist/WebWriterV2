namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RpgInit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Event",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Desc = c.String(nullable: false),
                        RequrmentSex = c.Int(),
                        RequrmentRace = c.Int(),
                        ProgressChanging = c.Double(nullable: false),
                        Event_Id = c.Long(),
                        Quest_Id = c.Long(nullable: false),
                        Quest_Id1 = c.Long(),
                        RequrmentLocation_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Event", t => t.Event_Id)
                .ForeignKey("dbo.Quest", t => t.Quest_Id)
                .ForeignKey("dbo.Quest", t => t.Quest_Id1)
                .ForeignKey("dbo.Location", t => t.RequrmentLocation_Id)
                .Index(t => t.Event_Id)
                .Index(t => t.Quest_Id)
                .Index(t => t.Quest_Id1)
                .Index(t => t.RequrmentLocation_Id);
            
            CreateTable(
                "dbo.Quest",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Desc = c.String(nullable: false),
                        Effective = c.Double(nullable: false),
                        Executor_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hero", t => t.Executor_Id)
                .Index(t => t.Executor_Id);
            
            CreateTable(
                "dbo.Hero",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Race = c.Int(nullable: false),
                        Sex = c.Int(nullable: false),
                        Background = c.String(),
                        Guild_Id = c.Long(),
                        Location_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Guild", t => t.Guild_Id)
                .ForeignKey("dbo.Location", t => t.Location_Id, cascadeDelete: true)
                .Index(t => t.Guild_Id)
                .Index(t => t.Location_Id);
            
            CreateTable(
                "dbo.Thing",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Owner_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hero", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Desc = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Guild",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        Name = c.String(nullable: false),
                        Desc = c.String(nullable: false),
                        Gold = c.Long(nullable: false),
                        Influence = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Location", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.TrainingRoom",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Price = c.Long(nullable: false),
                        School = c.Int(nullable: false),
                        Guild_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Guild", t => t.Guild_Id)
                .Index(t => t.Guild_Id);
            
            CreateTable(
                "dbo.Skill",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Desc = c.String(nullable: false),
                        School = c.Int(nullable: false),
                        Self_Id = c.Long(),
                        Target_Id = c.Long(),
                        Event_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hero", t => t.Self_Id)
                .ForeignKey("dbo.Hero", t => t.Target_Id)
                .ForeignKey("dbo.Event", t => t.Event_Id)
                .Index(t => t.Self_Id)
                .Index(t => t.Target_Id)
                .Index(t => t.Event_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Skill", "Event_Id", "dbo.Event");
            DropForeignKey("dbo.Skill", "Target_Id", "dbo.Hero");
            DropForeignKey("dbo.Skill", "Self_Id", "dbo.Hero");
            DropForeignKey("dbo.Event", "RequrmentLocation_Id", "dbo.Location");
            DropForeignKey("dbo.Event", "Quest_Id1", "dbo.Quest");
            DropForeignKey("dbo.Quest", "Executor_Id", "dbo.Hero");
            DropForeignKey("dbo.Hero", "Location_Id", "dbo.Location");
            DropForeignKey("dbo.TrainingRoom", "Guild_Id", "dbo.Guild");
            DropForeignKey("dbo.Guild", "Id", "dbo.Location");
            DropForeignKey("dbo.Hero", "Guild_Id", "dbo.Guild");
            DropForeignKey("dbo.Thing", "Owner_Id", "dbo.Hero");
            DropForeignKey("dbo.Event", "Quest_Id", "dbo.Quest");
            DropForeignKey("dbo.Event", "Event_Id", "dbo.Event");
            DropIndex("dbo.Skill", new[] { "Event_Id" });
            DropIndex("dbo.Skill", new[] { "Target_Id" });
            DropIndex("dbo.Skill", new[] { "Self_Id" });
            DropIndex("dbo.TrainingRoom", new[] { "Guild_Id" });
            DropIndex("dbo.Guild", new[] { "Id" });
            DropIndex("dbo.Thing", new[] { "Owner_Id" });
            DropIndex("dbo.Hero", new[] { "Location_Id" });
            DropIndex("dbo.Hero", new[] { "Guild_Id" });
            DropIndex("dbo.Quest", new[] { "Executor_Id" });
            DropIndex("dbo.Event", new[] { "RequrmentLocation_Id" });
            DropIndex("dbo.Event", new[] { "Quest_Id1" });
            DropIndex("dbo.Event", new[] { "Quest_Id" });
            DropIndex("dbo.Event", new[] { "Event_Id" });
            DropTable("dbo.Skill");
            DropTable("dbo.TrainingRoom");
            DropTable("dbo.Guild");
            DropTable("dbo.Location");
            DropTable("dbo.Thing");
            DropTable("dbo.Hero");
            DropTable("dbo.Quest");
            DropTable("dbo.Event");
        }
    }
}
