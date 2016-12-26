namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPriceForSkill : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Skill", "Price", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Skill", "Price");
        }
    }
}
