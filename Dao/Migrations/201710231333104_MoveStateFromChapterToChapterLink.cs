namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveStateFromChapterToChapterLink : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EventLinkItem", newName: "ChapterLinkItem");
            DropForeignKey("dbo.ChapterState", "Chapter_Id", "dbo.Chapter");
            DropForeignKey("dbo.ChapterState", "State_Id", "dbo.State");
            DropForeignKey("dbo.ChapterState1", "Chapter_Id", "dbo.Chapter");
            DropForeignKey("dbo.ChapterState1", "State_Id", "dbo.State");
            DropForeignKey("dbo.Thing", "Chapter_Id", "dbo.Chapter");
            DropForeignKey("dbo.Thing", "Chapter_Id1", "dbo.Chapter");
            DropIndex("dbo.Thing", new[] { "Chapter_Id" });
            DropIndex("dbo.Thing", new[] { "Chapter_Id1" });
            DropIndex("dbo.ChapterState", new[] { "Chapter_Id" });
            DropIndex("dbo.ChapterState", new[] { "State_Id" });
            DropIndex("dbo.ChapterState1", new[] { "Chapter_Id" });
            DropIndex("dbo.ChapterState1", new[] { "State_Id" });
            CreateTable(
                "dbo.ChapterLinkItemState",
                c => new
                    {
                        ChapterLinkItem_Id = c.Long(nullable: false),
                        State_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChapterLinkItem_Id, t.State_Id })
                .ForeignKey("dbo.ChapterLinkItem", t => t.ChapterLinkItem_Id, cascadeDelete: true)
                .ForeignKey("dbo.State", t => t.State_Id, cascadeDelete: true)
                .Index(t => t.ChapterLinkItem_Id)
                .Index(t => t.State_Id);
            
            CreateTable(
                "dbo.ChapterLinkItemState1",
                c => new
                    {
                        ChapterLinkItem_Id = c.Long(nullable: false),
                        State_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChapterLinkItem_Id, t.State_Id })
                .ForeignKey("dbo.ChapterLinkItem", t => t.ChapterLinkItem_Id, cascadeDelete: true)
                .ForeignKey("dbo.State", t => t.State_Id, cascadeDelete: true)
                .Index(t => t.ChapterLinkItem_Id)
                .Index(t => t.State_Id);
            
            AddColumn("dbo.Thing", "ChapterLinkItem_Id", c => c.Long());
            AddColumn("dbo.Thing", "ChapterLinkItem_Id1", c => c.Long());
            CreateIndex("dbo.Thing", "ChapterLinkItem_Id");
            CreateIndex("dbo.Thing", "ChapterLinkItem_Id1");
            AddForeignKey("dbo.Thing", "ChapterLinkItem_Id", "dbo.ChapterLinkItem", "Id");
            AddForeignKey("dbo.Thing", "ChapterLinkItem_Id1", "dbo.ChapterLinkItem", "Id");
            DropColumn("dbo.Chapter", "ProgressChanging");
            DropColumn("dbo.Thing", "Chapter_Id");
            DropColumn("dbo.Thing", "Chapter_Id1");
            DropTable("dbo.ChapterState");
            DropTable("dbo.ChapterState1");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ChapterState1",
                c => new
                    {
                        Chapter_Id = c.Long(nullable: false),
                        State_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Chapter_Id, t.State_Id });
            
            CreateTable(
                "dbo.ChapterState",
                c => new
                    {
                        Chapter_Id = c.Long(nullable: false),
                        State_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Chapter_Id, t.State_Id });
            
            AddColumn("dbo.Thing", "Chapter_Id1", c => c.Long());
            AddColumn("dbo.Thing", "Chapter_Id", c => c.Long());
            AddColumn("dbo.Chapter", "ProgressChanging", c => c.Double(nullable: false));
            DropForeignKey("dbo.Thing", "ChapterLinkItem_Id1", "dbo.ChapterLinkItem");
            DropForeignKey("dbo.Thing", "ChapterLinkItem_Id", "dbo.ChapterLinkItem");
            DropForeignKey("dbo.ChapterLinkItemState1", "State_Id", "dbo.State");
            DropForeignKey("dbo.ChapterLinkItemState1", "ChapterLinkItem_Id", "dbo.ChapterLinkItem");
            DropForeignKey("dbo.ChapterLinkItemState", "State_Id", "dbo.State");
            DropForeignKey("dbo.ChapterLinkItemState", "ChapterLinkItem_Id", "dbo.ChapterLinkItem");
            DropIndex("dbo.ChapterLinkItemState1", new[] { "State_Id" });
            DropIndex("dbo.ChapterLinkItemState1", new[] { "ChapterLinkItem_Id" });
            DropIndex("dbo.ChapterLinkItemState", new[] { "State_Id" });
            DropIndex("dbo.ChapterLinkItemState", new[] { "ChapterLinkItem_Id" });
            DropIndex("dbo.Thing", new[] { "ChapterLinkItem_Id1" });
            DropIndex("dbo.Thing", new[] { "ChapterLinkItem_Id" });
            DropColumn("dbo.Thing", "ChapterLinkItem_Id1");
            DropColumn("dbo.Thing", "ChapterLinkItem_Id");
            DropTable("dbo.ChapterLinkItemState1");
            DropTable("dbo.ChapterLinkItemState");
            CreateIndex("dbo.ChapterState1", "State_Id");
            CreateIndex("dbo.ChapterState1", "Chapter_Id");
            CreateIndex("dbo.ChapterState", "State_Id");
            CreateIndex("dbo.ChapterState", "Chapter_Id");
            CreateIndex("dbo.Thing", "Chapter_Id1");
            CreateIndex("dbo.Thing", "Chapter_Id");
            AddForeignKey("dbo.Thing", "Chapter_Id1", "dbo.Chapter", "Id");
            AddForeignKey("dbo.Thing", "Chapter_Id", "dbo.Chapter", "Id");
            AddForeignKey("dbo.ChapterState1", "State_Id", "dbo.State", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ChapterState1", "Chapter_Id", "dbo.Chapter", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ChapterState", "State_Id", "dbo.State", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ChapterState", "Chapter_Id", "dbo.Chapter", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.ChapterLinkItem", newName: "EventLinkItem");
        }
    }
}
