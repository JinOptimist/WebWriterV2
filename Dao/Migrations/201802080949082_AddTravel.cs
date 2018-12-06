namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTravel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Hero", "CurrentChapter_Id", "dbo.Chapter");
            DropForeignKey("dbo.StateValue", "Hero_Id", "dbo.Hero");
            DropForeignKey("dbo.Hero", "Owner_Id", "dbo.User");
            DropIndex("dbo.Hero", new[] { "CurrentChapter_Id" });
            DropIndex("dbo.Hero", new[] { "Owner_Id" });
            DropIndex("dbo.StateValue", new[] { "Hero_Id" });
            CreateTable(
                "dbo.Travel",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        FinishTime = c.DateTime(),
                        CurrentChapter_Id = c.Long(),
                        Reader_Id = c.Long(),
                        Book_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Chapter", t => t.CurrentChapter_Id)
                .ForeignKey("dbo.User", t => t.Reader_Id)
                .ForeignKey("dbo.Book", t => t.Book_Id)
                .Index(t => t.CurrentChapter_Id)
                .Index(t => t.Reader_Id)
                .Index(t => t.Book_Id);
            
            CreateTable(
                "dbo.TravelStep",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        Travel_Id = c.Long(nullable: false),
                        Сhoice_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Travel", t => t.Travel_Id, cascadeDelete: true)
                .ForeignKey("dbo.ChapterLinkItem", t => t.Сhoice_Id)
                .Index(t => t.Travel_Id)
                .Index(t => t.Сhoice_Id);
            
            AddColumn("dbo.StateValue", "Travel_Id", c => c.Long());
            CreateIndex("dbo.StateValue", "Travel_Id");
            AddForeignKey("dbo.StateValue", "Travel_Id", "dbo.Travel", "Id");
            DropColumn("dbo.StateValue", "Hero_Id");
            DropTable("dbo.Hero");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Hero",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TimeCreation = c.DateTime(nullable: false),
                        LastChanges = c.DateTime(nullable: false),
                        CurrentChapter_Id = c.Long(),
                        Owner_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.StateValue", "Hero_Id", c => c.Long());
            DropForeignKey("dbo.Travel", "Book_Id", "dbo.Book");
            DropForeignKey("dbo.TravelStep", "Сhoice_Id", "dbo.ChapterLinkItem");
            DropForeignKey("dbo.Travel", "Reader_Id", "dbo.User");
            DropForeignKey("dbo.TravelStep", "Travel_Id", "dbo.Travel");
            DropForeignKey("dbo.StateValue", "Travel_Id", "dbo.Travel");
            DropForeignKey("dbo.Travel", "CurrentChapter_Id", "dbo.Chapter");
            DropIndex("dbo.TravelStep", new[] { "Сhoice_Id" });
            DropIndex("dbo.TravelStep", new[] { "Travel_Id" });
            DropIndex("dbo.StateValue", new[] { "Travel_Id" });
            DropIndex("dbo.Travel", new[] { "Book_Id" });
            DropIndex("dbo.Travel", new[] { "Reader_Id" });
            DropIndex("dbo.Travel", new[] { "CurrentChapter_Id" });
            DropColumn("dbo.StateValue", "Travel_Id");
            DropTable("dbo.TravelStep");
            DropTable("dbo.Travel");
            CreateIndex("dbo.StateValue", "Hero_Id");
            CreateIndex("dbo.Hero", "Owner_Id");
            CreateIndex("dbo.Hero", "CurrentChapter_Id");
            AddForeignKey("dbo.Hero", "Owner_Id", "dbo.User", "Id", cascadeDelete: true);
            AddForeignKey("dbo.StateValue", "Hero_Id", "dbo.Hero", "Id");
            AddForeignKey("dbo.Hero", "CurrentChapter_Id", "dbo.Chapter", "Id");
        }
    }
}
