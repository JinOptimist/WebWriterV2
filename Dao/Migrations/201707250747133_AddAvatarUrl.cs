namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAvatarUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "AvatarUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "AvatarUrl");
        }
    }
}
