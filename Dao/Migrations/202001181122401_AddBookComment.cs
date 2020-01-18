namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBookComment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookComment",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        PublicationDate = c.DateTime(nullable: false),
                        Book_Id = c.Long(nullable: false),
                        Author_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Book", t => t.Book_Id, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.Author_Id)
                .Index(t => t.Book_Id)
                .Index(t => t.Author_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookComment", "Author_Id", "dbo.User");
            DropForeignKey("dbo.BookComment", "Book_Id", "dbo.Book");
            DropIndex("dbo.BookComment", new[] { "Author_Id" });
            DropIndex("dbo.BookComment", new[] { "Book_Id" });
            DropTable("dbo.BookComment");
        }
    }
}
