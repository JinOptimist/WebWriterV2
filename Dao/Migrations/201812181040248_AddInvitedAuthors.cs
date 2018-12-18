namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvitedAuthors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookUser",
                c => new
                    {
                        Book_Id = c.Long(nullable: false),
                        User_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Book_Id, t.User_Id })
                .ForeignKey("dbo.Book", t => t.Book_Id, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_Id, cascadeDelete: false)
                .Index(t => t.Book_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookUser", "User_Id", "dbo.User");
            DropForeignKey("dbo.BookUser", "Book_Id", "dbo.Book");
            DropIndex("dbo.BookUser", new[] { "User_Id" });
            DropIndex("dbo.BookUser", new[] { "Book_Id" });
            DropTable("dbo.BookUser");
        }
    }
}
