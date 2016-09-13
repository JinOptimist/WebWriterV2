namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class HellTypeStateAndChara : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HeroState", "Hero_Id", "dbo.Hero");
            DropForeignKey("dbo.HeroState", "State_Id", "dbo.State");
            DropForeignKey("dbo.State", "Skill_Id1", "dbo.Skill");
            DropForeignKey("dbo.Hero", "Guild_Id", "dbo.Guild");
            DropIndex("dbo.HeroState", new[] { "Hero_Id" });
            DropIndex("dbo.HeroState", new[] { "State_Id" });
            CreateTable(
                "dbo.CharacteristicType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Desc = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.StateType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Desc = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Thing",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Desc = c.String(),
                        IsUsed = c.Boolean(nullable: false),
                        Owner_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hero", t => t.Owner_Id)
                .Index(t => t.Owner_Id);

            AddColumn("dbo.Characteristic", "CharacteristicType_Id", c => c.Long());
            AddColumn("dbo.Characteristic", "Thing_Id", c => c.Long());
            AddColumn("dbo.State", "StateType_Id", c => c.Long());
            AddColumn("dbo.State", "CharacteristicType_Id", c => c.Long());
            AddColumn("dbo.State", "Hero_Id", c => c.Long());
            AddColumn("dbo.State", "Thing_Id", c => c.Long());
            CreateIndex("dbo.Characteristic", "CharacteristicType_Id");
            CreateIndex("dbo.Characteristic", "Thing_Id");
            CreateIndex("dbo.State", "StateType_Id");
            CreateIndex("dbo.State", "CharacteristicType_Id");
            CreateIndex("dbo.State", "Hero_Id");
            CreateIndex("dbo.State", "Thing_Id");
            AddForeignKey("dbo.State", "StateType_Id", "dbo.StateType", "Id");
            AddForeignKey("dbo.State", "CharacteristicType_Id", "dbo.CharacteristicType", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Characteristic", "CharacteristicType_Id", "dbo.CharacteristicType", "Id");
            AddForeignKey("dbo.State", "Hero_Id", "dbo.Hero", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Characteristic", "Thing_Id", "dbo.Thing", "Id", cascadeDelete: true);
            AddForeignKey("dbo.State", "Thing_Id", "dbo.Thing", "Id", cascadeDelete: true);
            AddForeignKey("dbo.State", "Skill_Id1", "dbo.Skill", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Hero", "Guild_Id", "dbo.Guild", "Id", cascadeDelete: true);
            DropColumn("dbo.Characteristic", "CharacteristicType");
            DropColumn("dbo.State", "StateType");
            DropTable("dbo.HeroState");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.HeroState",
                c => new
                    {
                        Hero_Id = c.Long(nullable: false),
                        State_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Hero_Id, t.State_Id });

            AddColumn("dbo.State", "StateType", c => c.Int(nullable: false));
            AddColumn("dbo.Characteristic", "CharacteristicType", c => c.Int(nullable: false));
            DropForeignKey("dbo.Hero", "Guild_Id", "dbo.Guild");
            DropForeignKey("dbo.State", "Skill_Id1", "dbo.Skill");
            DropForeignKey("dbo.Thing", "Owner_Id", "dbo.Hero");
            DropForeignKey("dbo.State", "Thing_Id", "dbo.Thing");
            DropForeignKey("dbo.Characteristic", "Thing_Id", "dbo.Thing");
            DropForeignKey("dbo.State", "Hero_Id", "dbo.Hero");
            DropForeignKey("dbo.Characteristic", "CharacteristicType_Id", "dbo.CharacteristicType");
            DropForeignKey("dbo.State", "CharacteristicType_Id", "dbo.CharacteristicType");
            DropForeignKey("dbo.State", "StateType_Id", "dbo.StateType");
            DropIndex("dbo.Thing", new[] { "Owner_Id" });
            DropIndex("dbo.State", new[] { "Thing_Id" });
            DropIndex("dbo.State", new[] { "Hero_Id" });
            DropIndex("dbo.State", new[] { "CharacteristicType_Id" });
            DropIndex("dbo.State", new[] { "StateType_Id" });
            DropIndex("dbo.Characteristic", new[] { "Thing_Id" });
            DropIndex("dbo.Characteristic", new[] { "CharacteristicType_Id" });
            DropColumn("dbo.State", "Thing_Id");
            DropColumn("dbo.State", "Hero_Id");
            DropColumn("dbo.State", "CharacteristicType_Id");
            DropColumn("dbo.State", "StateType_Id");
            DropColumn("dbo.Characteristic", "Thing_Id");
            DropColumn("dbo.Characteristic", "CharacteristicType_Id");
            DropTable("dbo.Thing");
            DropTable("dbo.StateType");
            DropTable("dbo.CharacteristicType");
            CreateIndex("dbo.HeroState", "State_Id");
            CreateIndex("dbo.HeroState", "Hero_Id");
            AddForeignKey("dbo.Hero", "Guild_Id", "dbo.Guild", "Id");
            AddForeignKey("dbo.State", "Skill_Id1", "dbo.Skill", "Id");
            AddForeignKey("dbo.HeroState", "State_Id", "dbo.State", "Id", cascadeDelete: true);
            AddForeignKey("dbo.HeroState", "Hero_Id", "dbo.Hero", "Id", cascadeDelete: true);
        }
    }
}
