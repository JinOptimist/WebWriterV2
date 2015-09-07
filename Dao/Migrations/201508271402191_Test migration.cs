namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Testmigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserFromVk", "NewTestField", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserFromVk", "NewTestField");
        }
    }
}
