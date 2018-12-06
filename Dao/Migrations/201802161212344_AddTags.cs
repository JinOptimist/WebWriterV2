namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTags : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.BookTag",
                c => new
                    {
                        Book_Id = c.Long(nullable: false),
                        Tag_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Book_Id, t.Tag_Id })
                .ForeignKey("dbo.Book", t => t.Book_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tag", t => t.Tag_Id, cascadeDelete: true)
                .Index(t => t.Book_Id)
                .Index(t => t.Tag_Id);
            
            AlterColumn("dbo.Genre", "Name", c => c.String(maxLength: 500));
            CreateIndex("dbo.Genre", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookTag", "Tag_Id", "dbo.Tag");
            DropForeignKey("dbo.BookTag", "Book_Id", "dbo.Book");
            DropIndex("dbo.BookTag", new[] { "Tag_Id" });
            DropIndex("dbo.BookTag", new[] { "Book_Id" });
            DropIndex("dbo.Tag", new[] { "Name" });
            DropIndex("dbo.Genre", new[] { "Name" });
            AlterColumn("dbo.Genre", "Name", c => c.String());
            DropTable("dbo.BookTag");
            DropTable("dbo.Tag");
        }
    }
}
