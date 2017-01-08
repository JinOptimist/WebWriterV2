namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequirementType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Thing", "RequirementType", c => c.Int());
            AddColumn("dbo.Characteristic", "RequirementType", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Characteristic", "RequirementType");
            DropColumn("dbo.Thing", "RequirementType");
        }
    }
}
