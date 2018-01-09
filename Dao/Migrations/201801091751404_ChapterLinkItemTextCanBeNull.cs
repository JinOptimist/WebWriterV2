namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChapterLinkItemTextCanBeNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ChapterLinkItem", "Text", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ChapterLinkItem", "Text", c => c.String(nullable: false));
        }
    }
}
