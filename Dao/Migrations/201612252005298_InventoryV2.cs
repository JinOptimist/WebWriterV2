namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InventoryV2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ThingSample", "RequrmentSex", c => c.Int(nullable: false));
            AddColumn("dbo.ThingSample", "RequrmentRace", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ThingSample", "RequrmentRace");
            DropColumn("dbo.ThingSample", "RequrmentSex");
        }
    }
}
