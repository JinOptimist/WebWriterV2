namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SimplestHeroAndSKill : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Skill", "Self_Id", "dbo.Hero");
            DropForeignKey("dbo.Skill", "Target_Id", "dbo.Hero");
            DropIndex("dbo.Skill", new[] { "Self_Id" });
            DropIndex("dbo.Skill", new[] { "Target_Id" });
            DropColumn("dbo.Skill", "Self_Id");
            DropColumn("dbo.Skill", "Target_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Skill", "Target_Id", c => c.Long());
            AddColumn("dbo.Skill", "Self_Id", c => c.Long());
            CreateIndex("dbo.Skill", "Target_Id");
            CreateIndex("dbo.Skill", "Self_Id");
            AddForeignKey("dbo.Skill", "Target_Id", "dbo.Hero", "Id");
            AddForeignKey("dbo.Skill", "Self_Id", "dbo.Hero", "Id");
        }
    }
}
