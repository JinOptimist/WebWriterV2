namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateChapter : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Chapter", "Id", "dbo.Book");
            DropIndex("dbo.Chapter", new[] { "Id" });
            AddColumn("dbo.Chapter", "ForRootBook_Id", c => c.Long());
            CreateIndex("dbo.Chapter", "ForRootBook_Id");
            AddForeignKey("dbo.Chapter", "ForRootBook_Id", "dbo.Book", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chapter", "ForRootBook_Id", "dbo.Book");
            DropIndex("dbo.Chapter", new[] { "ForRootBook_Id" });
            DropColumn("dbo.Chapter", "ForRootBook_Id");
            CreateIndex("dbo.Chapter", "Id");
            AddForeignKey("dbo.Chapter", "Id", "dbo.Book", "Id");
        }
    }
}
