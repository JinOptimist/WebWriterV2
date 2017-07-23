namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsPublishedToBook : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Book", "IsPublished", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Book", "IsPublished");
        }
    }
}
