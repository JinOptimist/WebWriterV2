namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNumberOfWordToChapterAndBook : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Event", "NumberOfWords", c => c.Long(nullable: false));
            AddColumn("dbo.Book", "NumberOfChapters", c => c.Long(nullable: false));
            AddColumn("dbo.Book", "NumberOfWords", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Book", "NumberOfWords");
            DropColumn("dbo.Book", "NumberOfChapters");
            DropColumn("dbo.Event", "NumberOfWords");
        }
    }
}
