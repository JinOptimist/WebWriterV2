namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Deletetestfield : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserFromVk", "NewTestField");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserFromVk", "NewTestField", c => c.String());
        }
    }
}
