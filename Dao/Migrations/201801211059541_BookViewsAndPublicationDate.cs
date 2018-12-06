namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookViewsAndPublicationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Book", "PublicationDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Book", "Views", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Book", "Views");
            DropColumn("dbo.Book", "PublicationDate");
        }
    }
}
