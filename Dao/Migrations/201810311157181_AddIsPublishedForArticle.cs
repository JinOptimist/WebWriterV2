namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsPublishedForArticle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Article", "IsPublished", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Article", "IsPublished");
        }
    }
}
