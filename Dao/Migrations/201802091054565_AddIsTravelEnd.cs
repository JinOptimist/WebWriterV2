namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsTravelEnd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Travel", "IsTravelEnd", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Travel", "IsTravelEnd");
        }
    }
}
