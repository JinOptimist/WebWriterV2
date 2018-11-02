namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addShortDescForArticle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Article", "ShortDesc", c => c.String(nullable: false, maxLength: 300));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Article", "ShortDesc");
        }
    }
}
