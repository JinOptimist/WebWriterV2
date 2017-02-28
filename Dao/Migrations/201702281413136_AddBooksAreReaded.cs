namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddBooksAreReaded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserQuest",
                c => new
                    {
                        User_Id = c.Long(nullable: false),
                        Quest_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Quest_Id })
                .ForeignKey("dbo.User", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Quest", t => t.Quest_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Quest_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.UserQuest", "Quest_Id", "dbo.Quest");
            DropForeignKey("dbo.UserQuest", "User_Id", "dbo.User");
            DropIndex("dbo.UserQuest", new[] { "Quest_Id" });
            DropIndex("dbo.UserQuest", new[] { "User_Id" });
            DropTable("dbo.UserQuest");
        }
    }
}
