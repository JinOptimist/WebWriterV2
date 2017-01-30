namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdminBoolean : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "IsAdmin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "IsAdmin");
        }
    }
}
