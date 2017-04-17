namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHideFromReaderToStateType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StateType", "HideFromReader", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StateType", "HideFromReader");
        }
    }
}
