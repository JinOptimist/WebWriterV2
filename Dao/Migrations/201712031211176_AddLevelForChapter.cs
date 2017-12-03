namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLevelForChapter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chapter", "Level", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chapter", "Level");
        }
    }
}
