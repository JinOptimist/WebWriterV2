namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class BigClean : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.State", "CharacteristicType_Id", "dbo.CharacteristicType");
            DropForeignKey("dbo.Characteristic", "CharacteristicType_Id", "dbo.CharacteristicType");
            DropForeignKey("dbo.Characteristic", "Hero_Id", "dbo.Hero");
            DropForeignKey("dbo.Characteristic", "Event_Id", "dbo.Event");
            DropForeignKey("dbo.Quest", "Executor_Id", "dbo.Hero");
            DropForeignKey("dbo.EventCharacteristic", "Event_Id", "dbo.Event");
            DropForeignKey("dbo.EventCharacteristic", "Characteristic_Id", "dbo.Characteristic");
            DropForeignKey("dbo.Skill", "School_Id", "dbo.SkillSchool");
            DropForeignKey("dbo.State", "Skill_Id", "dbo.Skill");
            DropForeignKey("dbo.State", "Skill_Id1", "dbo.Skill");
            DropForeignKey("dbo.EventSkill", "Event_Id", "dbo.Event");
            DropForeignKey("dbo.EventSkill", "Skill_Id", "dbo.Skill");
            DropForeignKey("dbo.Characteristic", "ThingSample_Id", "dbo.ThingSample");
            DropForeignKey("dbo.Characteristic", "ThingSample_Id1", "dbo.ThingSample");
            DropForeignKey("dbo.Hero", "Guild_Id", "dbo.Guild");
            DropForeignKey("dbo.TrainingRoom", "School_Id", "dbo.SkillSchool");
            DropForeignKey("dbo.TrainingRoom", "Guild_Id", "dbo.Guild");
            DropForeignKey("dbo.HeroSkill", "Hero_Id", "dbo.Hero");
            DropForeignKey("dbo.HeroSkill", "Skill_Id", "dbo.Skill");
            DropIndex("dbo.Hero", new[] { "Guild_Id" });
            DropIndex("dbo.Characteristic", new[] { "CharacteristicType_Id" });
            DropIndex("dbo.Characteristic", new[] { "Hero_Id" });
            DropIndex("dbo.Characteristic", new[] { "Event_Id" });
            DropIndex("dbo.Characteristic", new[] { "ThingSample_Id" });
            DropIndex("dbo.Characteristic", new[] { "ThingSample_Id1" });
            DropIndex("dbo.CharacteristicType", new[] { "Name" });
            DropIndex("dbo.State", new[] { "CharacteristicType_Id" });
            DropIndex("dbo.State", new[] { "Skill_Id" });
            DropIndex("dbo.State", new[] { "Skill_Id1" });
            DropIndex("dbo.Quest", new[] { "Executor_Id" });
            DropIndex("dbo.Skill", new[] { "Name" });
            DropIndex("dbo.Skill", new[] { "School_Id" });
            DropIndex("dbo.SkillSchool", new[] { "Name" });
            DropIndex("dbo.TrainingRoom", new[] { "Name" });
            DropIndex("dbo.TrainingRoom", new[] { "School_Id" });
            DropIndex("dbo.TrainingRoom", new[] { "Guild_Id" });
            DropIndex("dbo.EventCharacteristic", new[] { "Event_Id" });
            DropIndex("dbo.EventCharacteristic", new[] { "Characteristic_Id" });
            DropIndex("dbo.EventSkill", new[] { "Event_Id" });
            DropIndex("dbo.EventSkill", new[] { "Skill_Id" });
            DropIndex("dbo.HeroSkill", new[] { "Hero_Id" });
            DropIndex("dbo.HeroSkill", new[] { "Skill_Id" });
            DropColumn("dbo.Hero", "Race");
            DropColumn("dbo.Hero", "Sex");
            DropColumn("dbo.Hero", "Guild_Id");
            DropColumn("dbo.State", "CharacteristicType_Id");
            DropColumn("dbo.State", "Skill_Id");
            DropColumn("dbo.State", "Skill_Id1");
            DropColumn("dbo.Event", "RequirementSex");
            DropColumn("dbo.Event", "RequirementRace");
            DropColumn("dbo.Quest", "Effective");
            DropColumn("dbo.Quest", "Executor_Id");
            DropColumn("dbo.ThingSample", "RequirementSex");
            DropColumn("dbo.ThingSample", "RequirementRace");
            DropTable("dbo.Characteristic");
            DropTable("dbo.CharacteristicType");
            DropTable("dbo.Skill");
            DropTable("dbo.SkillSchool");
            DropTable("dbo.Guild");
            DropTable("dbo.TrainingRoom");
            DropTable("dbo.EventCharacteristic");
            DropTable("dbo.EventSkill");
            DropTable("dbo.HeroSkill");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.HeroSkill",
                c => new
                    {
                        Hero_Id = c.Long(nullable: false),
                        Skill_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Hero_Id, t.Skill_Id });

            CreateTable(
                "dbo.EventSkill",
                c => new
                    {
                        Event_Id = c.Long(nullable: false),
                        Skill_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Event_Id, t.Skill_Id });

            CreateTable(
                "dbo.EventCharacteristic",
                c => new
                    {
                        Event_Id = c.Long(nullable: false),
                        Characteristic_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Event_Id, t.Characteristic_Id });

            CreateTable(
                "dbo.TrainingRoom",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 120),
                        Price = c.Long(nullable: false),
                        School_Id = c.Long(),
                        Guild_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Guild",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 120),
                        Desc = c.String(),
                        Gold = c.Long(nullable: false),
                        Influence = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.SkillSchool",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 120),
                        Desc = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Skill",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 120),
                        Desc = c.String(nullable: false),
                        Price = c.Int(nullable: false),
                        School_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.CharacteristicType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 120),
                        Desc = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Characteristic",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Number = c.Long(nullable: false),
                        RequirementType = c.Int(),
                        CharacteristicType_Id = c.Long(),
                        Hero_Id = c.Long(),
                        Event_Id = c.Long(),
                        ThingSample_Id = c.Long(),
                        ThingSample_Id1 = c.Long(),
                    })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.ThingSample", "RequirementRace", c => c.Int(nullable: false));
            AddColumn("dbo.ThingSample", "RequirementSex", c => c.Int(nullable: false));
            AddColumn("dbo.Quest", "Executor_Id", c => c.Long());
            AddColumn("dbo.Quest", "Effective", c => c.Double(nullable: false));
            AddColumn("dbo.Event", "RequirementRace", c => c.Int());
            AddColumn("dbo.Event", "RequirementSex", c => c.Int());
            AddColumn("dbo.State", "Skill_Id1", c => c.Long());
            AddColumn("dbo.State", "Skill_Id", c => c.Long());
            AddColumn("dbo.State", "CharacteristicType_Id", c => c.Long());
            AddColumn("dbo.Hero", "Guild_Id", c => c.Long());
            AddColumn("dbo.Hero", "Sex", c => c.Int(nullable: false));
            AddColumn("dbo.Hero", "Race", c => c.Int(nullable: false));
            CreateIndex("dbo.HeroSkill", "Skill_Id");
            CreateIndex("dbo.HeroSkill", "Hero_Id");
            CreateIndex("dbo.EventSkill", "Skill_Id");
            CreateIndex("dbo.EventSkill", "Event_Id");
            CreateIndex("dbo.EventCharacteristic", "Characteristic_Id");
            CreateIndex("dbo.EventCharacteristic", "Event_Id");
            CreateIndex("dbo.TrainingRoom", "Guild_Id");
            CreateIndex("dbo.TrainingRoom", "School_Id");
            CreateIndex("dbo.TrainingRoom", "Name", unique: true);
            CreateIndex("dbo.SkillSchool", "Name", unique: true);
            CreateIndex("dbo.Skill", "School_Id");
            CreateIndex("dbo.Skill", "Name", unique: true);
            CreateIndex("dbo.Quest", "Executor_Id");
            CreateIndex("dbo.State", "Skill_Id1");
            CreateIndex("dbo.State", "Skill_Id");
            CreateIndex("dbo.State", "CharacteristicType_Id");
            CreateIndex("dbo.CharacteristicType", "Name", unique: true);
            CreateIndex("dbo.Characteristic", "ThingSample_Id1");
            CreateIndex("dbo.Characteristic", "ThingSample_Id");
            CreateIndex("dbo.Characteristic", "Event_Id");
            CreateIndex("dbo.Characteristic", "Hero_Id");
            CreateIndex("dbo.Characteristic", "CharacteristicType_Id");
            CreateIndex("dbo.Hero", "Guild_Id");
            AddForeignKey("dbo.HeroSkill", "Skill_Id", "dbo.Skill", "Id", cascadeDelete: true);
            AddForeignKey("dbo.HeroSkill", "Hero_Id", "dbo.Hero", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TrainingRoom", "Guild_Id", "dbo.Guild", "Id");
            AddForeignKey("dbo.TrainingRoom", "School_Id", "dbo.SkillSchool", "Id");
            AddForeignKey("dbo.Hero", "Guild_Id", "dbo.Guild", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Characteristic", "ThingSample_Id1", "dbo.ThingSample", "Id");
            AddForeignKey("dbo.Characteristic", "ThingSample_Id", "dbo.ThingSample", "Id");
            AddForeignKey("dbo.EventSkill", "Skill_Id", "dbo.Skill", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EventSkill", "Event_Id", "dbo.Event", "Id", cascadeDelete: true);
            AddForeignKey("dbo.State", "Skill_Id1", "dbo.Skill", "Id", cascadeDelete: true);
            AddForeignKey("dbo.State", "Skill_Id", "dbo.Skill", "Id");
            AddForeignKey("dbo.Skill", "School_Id", "dbo.SkillSchool", "Id");
            AddForeignKey("dbo.EventCharacteristic", "Characteristic_Id", "dbo.Characteristic", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EventCharacteristic", "Event_Id", "dbo.Event", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Quest", "Executor_Id", "dbo.Hero", "Id");
            AddForeignKey("dbo.Characteristic", "Event_Id", "dbo.Event", "Id");
            AddForeignKey("dbo.Characteristic", "Hero_Id", "dbo.Hero", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Characteristic", "CharacteristicType_Id", "dbo.CharacteristicType", "Id");
            AddForeignKey("dbo.State", "CharacteristicType_Id", "dbo.CharacteristicType", "Id", cascadeDelete: true);
        }
    }
}
