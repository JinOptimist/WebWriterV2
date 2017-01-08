namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixMistypesRequirementV2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ThingSample", "RequirementSex", c => c.Int(nullable: false));
            AddColumn("dbo.ThingSample", "RequirementRace", c => c.Int(nullable: false));
            DropColumn("dbo.ThingSample", "RequrmentSex");
            DropColumn("dbo.ThingSample", "RequrmentRace");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ThingSample", "RequrmentRace", c => c.Int(nullable: false));
            AddColumn("dbo.ThingSample", "RequrmentSex", c => c.Int(nullable: false));
            DropColumn("dbo.ThingSample", "RequirementRace");
            DropColumn("dbo.ThingSample", "RequirementSex");
        }
    }
}
