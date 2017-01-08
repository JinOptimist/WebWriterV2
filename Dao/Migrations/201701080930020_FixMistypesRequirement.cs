namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixMistypesRequirement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Event", "RequirementSex", c => c.Int());
            AddColumn("dbo.Event", "RequirementRace", c => c.Int());
            DropColumn("dbo.Event", "RequrmentSex");
            DropColumn("dbo.Event", "RequrmentRace");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Event", "RequrmentRace", c => c.Int());
            AddColumn("dbo.Event", "RequrmentSex", c => c.Int());
            DropColumn("dbo.Event", "RequirementRace");
            DropColumn("dbo.Event", "RequirementSex");
        }
    }
}
