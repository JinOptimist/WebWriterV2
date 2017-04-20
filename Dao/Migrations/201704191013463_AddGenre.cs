namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGenre : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Genre",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Desc = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Quest", "Genre_Id", c => c.Long());
            CreateIndex("dbo.Quest", "Genre_Id");
            AddForeignKey("dbo.Quest", "Genre_Id", "dbo.Genre", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Quest", "Genre_Id", "dbo.Genre");
            DropIndex("dbo.Quest", new[] { "Genre_Id" });
            DropColumn("dbo.Quest", "Genre_Id");
            DropTable("dbo.Genre");
        }
    }
}
