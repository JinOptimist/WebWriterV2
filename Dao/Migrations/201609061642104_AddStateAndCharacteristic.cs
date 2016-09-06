namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddStateAndCharacteristic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characteristic", "CharacteristicType", c => c.Int(nullable: false));
            AddColumn("dbo.State", "StateType", c => c.Int(nullable: false));
            AddColumn("dbo.State", "Skill_Id", c => c.Long());
            AddColumn("dbo.State", "Skill_Id1", c => c.Long());
            CreateIndex("dbo.State", "Skill_Id");
            CreateIndex("dbo.State", "Skill_Id1");
            AddForeignKey("dbo.State", "Skill_Id", "dbo.Skill", "Id");
            AddForeignKey("dbo.State", "Skill_Id1", "dbo.Skill", "Id");
            DropColumn("dbo.Characteristic", "Type");
            DropColumn("dbo.State", "Type");
        }

        public override void Down()
        {
            AddColumn("dbo.State", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Characteristic", "Type", c => c.Int(nullable: false));
            DropForeignKey("dbo.State", "Skill_Id1", "dbo.Skill");
            DropForeignKey("dbo.State", "Skill_Id", "dbo.Skill");
            DropIndex("dbo.State", new[] { "Skill_Id1" });
            DropIndex("dbo.State", new[] { "Skill_Id" });
            DropColumn("dbo.State", "Skill_Id1");
            DropColumn("dbo.State", "Skill_Id");
            DropColumn("dbo.State", "StateType");
            DropColumn("dbo.Characteristic", "CharacteristicType");
        }
    }
}
