namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShowExtendedFunctionality : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "ShowExtendedFunctionality", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "ShowExtendedFunctionality");
        }
    }
}
