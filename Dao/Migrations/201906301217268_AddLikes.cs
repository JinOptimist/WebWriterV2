namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLikes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Like",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        User_Id = c.Long(nullable: false),
                        Book_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .ForeignKey("dbo.Book", t => t.Book_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Book_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Like", "Book_Id", "dbo.Book");
            DropForeignKey("dbo.Like", "User_Id", "dbo.User");
            DropIndex("dbo.Like", new[] { "Book_Id" });
            DropIndex("dbo.Like", new[] { "User_Id" });
            DropTable("dbo.Like");
        }
    }
}
