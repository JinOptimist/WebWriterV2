namespace Dao.Migrations
{
    using Model;
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddUserType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "UserType", c => c.Int(nullable: false, defaultValue: (int)UserType.Reader));
            DropColumn("dbo.User", "IsAdmin");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "IsAdmin", c => c.Boolean(nullable: false, defaultValue: false));
            DropColumn("dbo.User", "UserType");
        }
    }
}
