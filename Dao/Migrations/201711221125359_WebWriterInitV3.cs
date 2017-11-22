namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WebWriterInitV3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 120),
                        Email = c.String(),
                        ConfirmCode = c.String(),
                        Password = c.String(nullable: false),
                        UserType = c.Int(nullable: false),
                        AvatarUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Hero",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Background = c.String(),
                        TimeCreation = c.DateTime(nullable: false),
                        LastChanges = c.DateTime(nullable: false),
                        CurrentChapter_Id = c.Long(),
                        Owner_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Chapter", t => t.CurrentChapter_Id)
                .ForeignKey("dbo.User", t => t.Owner_Id, cascadeDelete: true)
                .Index(t => t.CurrentChapter_Id)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.Chapter",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Desc = c.String(nullable: false),
                        NumberOfWords = c.Long(nullable: false),
                        Book_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Book", t => t.Book_Id, cascadeDelete: true)
                .ForeignKey("dbo.Book", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.Book_Id);
            
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Desc = c.String(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        NumberOfChapters = c.Long(nullable: false),
                        NumberOfWords = c.Long(nullable: false),
                        Genre_Id = c.Long(),
                        Owner_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Genre", t => t.Genre_Id)
                .ForeignKey("dbo.User", t => t.Owner_Id, cascadeDelete: true)
                .Index(t => t.Genre_Id)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.Evaluation",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Mark = c.Long(nullable: false),
                        Comment = c.String(),
                        Created = c.DateTime(nullable: false),
                        Book_Id = c.Long(nullable: false),
                        Owner_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Book", t => t.Book_Id)
                .ForeignKey("dbo.User", t => t.Owner_Id)
                .Index(t => t.Book_Id)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.Genre",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Desc = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserWhoReadBook",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Book_Id = c.Long(nullable: false),
                        User_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Book", t => t.Book_Id, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.Book_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.ChapterLinkItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        From_Id = c.Long(),
                        To_Id = c.Long(),
                        Chapter_Id = c.Long(),
                        Chapter_Id1 = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Chapter", t => t.From_Id)
                .ForeignKey("dbo.Chapter", t => t.To_Id)
                .ForeignKey("dbo.Chapter", t => t.Chapter_Id)
                .ForeignKey("dbo.Chapter", t => t.Chapter_Id1)
                .Index(t => t.From_Id)
                .Index(t => t.To_Id)
                .Index(t => t.Chapter_Id)
                .Index(t => t.Chapter_Id1);
            
            CreateTable(
                "dbo.StateValue",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.Long(),
                        Hero_Id = c.Long(),
                        StateType_Id = c.Long(nullable: false),
                        ChapterLinkItem_Id = c.Long(),
                        ChapterLinkItem_Id1 = c.Long(),
                        ThingSample_Id = c.Long(),
                        ThingSample_Id1 = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hero", t => t.Hero_Id)
                .ForeignKey("dbo.StateType", t => t.StateType_Id, cascadeDelete: true)
                .ForeignKey("dbo.ChapterLinkItem", t => t.ChapterLinkItem_Id)
                .ForeignKey("dbo.ChapterLinkItem", t => t.ChapterLinkItem_Id1)
                .ForeignKey("dbo.ThingSample", t => t.ThingSample_Id)
                .ForeignKey("dbo.ThingSample", t => t.ThingSample_Id1)
                .Index(t => t.Hero_Id)
                .Index(t => t.StateType_Id)
                .Index(t => t.ChapterLinkItem_Id)
                .Index(t => t.ChapterLinkItem_Id1)
                .Index(t => t.ThingSample_Id)
                .Index(t => t.ThingSample_Id1);
            
            CreateTable(
                "dbo.StateType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 120),
                        Desc = c.String(),
                        HideFromReader = c.Boolean(nullable: false),
                        Owner_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.Owner_Id, cascadeDelete: true)
                .Index(t => t.Name)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.Thing",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ItemInUse = c.Boolean(nullable: false),
                        Count = c.Int(nullable: false),
                        RequirementType = c.Int(),
                        Hero_Id = c.Long(),
                        ThingSample_Id = c.Long(),
                        ChapterLinkItem_Id = c.Long(),
                        ChapterLinkItem_Id1 = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hero", t => t.Hero_Id)
                .ForeignKey("dbo.ThingSample", t => t.ThingSample_Id)
                .ForeignKey("dbo.ChapterLinkItem", t => t.ChapterLinkItem_Id)
                .ForeignKey("dbo.ChapterLinkItem", t => t.ChapterLinkItem_Id1)
                .Index(t => t.Hero_Id)
                .Index(t => t.ThingSample_Id)
                .Index(t => t.ChapterLinkItem_Id)
                .Index(t => t.ChapterLinkItem_Id1);
            
            CreateTable(
                "dbo.ThingSample",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Desc = c.String(),
                        IsUsed = c.Boolean(nullable: false),
                        Owner_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StateType", "Owner_Id", "dbo.User");
            DropForeignKey("dbo.Evaluation", "Owner_Id", "dbo.User");
            DropForeignKey("dbo.UserWhoReadBook", "User_Id", "dbo.User");
            DropForeignKey("dbo.Book", "Owner_Id", "dbo.User");
            DropForeignKey("dbo.Hero", "Owner_Id", "dbo.User");
            DropForeignKey("dbo.Hero", "CurrentChapter_Id", "dbo.Chapter");
            DropForeignKey("dbo.ChapterLinkItem", "Chapter_Id1", "dbo.Chapter");
            DropForeignKey("dbo.ChapterLinkItem", "Chapter_Id", "dbo.Chapter");
            DropForeignKey("dbo.ChapterLinkItem", "To_Id", "dbo.Chapter");
            DropForeignKey("dbo.Thing", "ChapterLinkItem_Id1", "dbo.ChapterLinkItem");
            DropForeignKey("dbo.Thing", "ChapterLinkItem_Id", "dbo.ChapterLinkItem");
            DropForeignKey("dbo.Thing", "ThingSample_Id", "dbo.ThingSample");
            DropForeignKey("dbo.StateValue", "ThingSample_Id1", "dbo.ThingSample");
            DropForeignKey("dbo.StateValue", "ThingSample_Id", "dbo.ThingSample");
            DropForeignKey("dbo.ThingSample", "Owner_Id", "dbo.User");
            DropForeignKey("dbo.Thing", "Hero_Id", "dbo.Hero");
            DropForeignKey("dbo.StateValue", "ChapterLinkItem_Id1", "dbo.ChapterLinkItem");
            DropForeignKey("dbo.StateValue", "ChapterLinkItem_Id", "dbo.ChapterLinkItem");
            DropForeignKey("dbo.StateValue", "StateType_Id", "dbo.StateType");
            DropForeignKey("dbo.StateValue", "Hero_Id", "dbo.Hero");
            DropForeignKey("dbo.ChapterLinkItem", "From_Id", "dbo.Chapter");
            DropForeignKey("dbo.Chapter", "Id", "dbo.Book");
            DropForeignKey("dbo.UserWhoReadBook", "Book_Id", "dbo.Book");
            DropForeignKey("dbo.Book", "Genre_Id", "dbo.Genre");
            DropForeignKey("dbo.Evaluation", "Book_Id", "dbo.Book");
            DropForeignKey("dbo.Chapter", "Book_Id", "dbo.Book");
            DropIndex("dbo.ThingSample", new[] { "Owner_Id" });
            DropIndex("dbo.Thing", new[] { "ChapterLinkItem_Id1" });
            DropIndex("dbo.Thing", new[] { "ChapterLinkItem_Id" });
            DropIndex("dbo.Thing", new[] { "ThingSample_Id" });
            DropIndex("dbo.Thing", new[] { "Hero_Id" });
            DropIndex("dbo.StateType", new[] { "Owner_Id" });
            DropIndex("dbo.StateType", new[] { "Name" });
            DropIndex("dbo.StateValue", new[] { "ThingSample_Id1" });
            DropIndex("dbo.StateValue", new[] { "ThingSample_Id" });
            DropIndex("dbo.StateValue", new[] { "ChapterLinkItem_Id1" });
            DropIndex("dbo.StateValue", new[] { "ChapterLinkItem_Id" });
            DropIndex("dbo.StateValue", new[] { "StateType_Id" });
            DropIndex("dbo.StateValue", new[] { "Hero_Id" });
            DropIndex("dbo.ChapterLinkItem", new[] { "Chapter_Id1" });
            DropIndex("dbo.ChapterLinkItem", new[] { "Chapter_Id" });
            DropIndex("dbo.ChapterLinkItem", new[] { "To_Id" });
            DropIndex("dbo.ChapterLinkItem", new[] { "From_Id" });
            DropIndex("dbo.UserWhoReadBook", new[] { "User_Id" });
            DropIndex("dbo.UserWhoReadBook", new[] { "Book_Id" });
            DropIndex("dbo.Evaluation", new[] { "Owner_Id" });
            DropIndex("dbo.Evaluation", new[] { "Book_Id" });
            DropIndex("dbo.Book", new[] { "Owner_Id" });
            DropIndex("dbo.Book", new[] { "Genre_Id" });
            DropIndex("dbo.Chapter", new[] { "Book_Id" });
            DropIndex("dbo.Chapter", new[] { "Id" });
            DropIndex("dbo.Hero", new[] { "Owner_Id" });
            DropIndex("dbo.Hero", new[] { "CurrentChapter_Id" });
            DropIndex("dbo.User", new[] { "Name" });
            DropTable("dbo.ThingSample");
            DropTable("dbo.Thing");
            DropTable("dbo.StateType");
            DropTable("dbo.StateValue");
            DropTable("dbo.ChapterLinkItem");
            DropTable("dbo.UserWhoReadBook");
            DropTable("dbo.Genre");
            DropTable("dbo.Evaluation");
            DropTable("dbo.Book");
            DropTable("dbo.Chapter");
            DropTable("dbo.Hero");
            DropTable("dbo.User");
        }
    }
}
