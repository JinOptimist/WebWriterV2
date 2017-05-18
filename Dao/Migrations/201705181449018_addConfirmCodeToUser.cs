namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addConfirmCodeToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "ConfirmCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "ConfirmCode");
        }
    }
}
