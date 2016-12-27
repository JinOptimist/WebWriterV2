namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class RpgInitV2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Quest",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Desc = c.String(nullable: false),
                        Effective = c.Double(nullable: false),
                        Executor_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hero", t => t.Executor_Id)
                .Index(t => t.Executor_Id);

            CreateTable(
                "dbo.Event",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Desc = c.String(nullable: false),
                        RequrmentSex = c.Int(),
                        RequrmentRace = c.Int(),
                        ProgressChanging = c.Double(nullable: false),
                        Quest_Id = c.Long(nullable: false),
                        ForRootQuest_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quest", t => t.Quest_Id)
                .ForeignKey("dbo.Quest", t => t.ForRootQuest_Id)
                .Index(t => t.Quest_Id)
                .Index(t => t.ForRootQuest_Id);

            CreateTable(
                "dbo.State",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Number = c.Long(nullable: false),
                        StateType_Id = c.Long(),
                        CharacteristicType_Id = c.Long(),
                        Skill_Id = c.Long(),
                        Skill_Id1 = c.Long(),
                        ThingSample_Id = c.Long(),
                        ThingSample_Id1 = c.Long(),
                        Hero_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StateType", t => t.StateType_Id)
                .ForeignKey("dbo.CharacteristicType", t => t.CharacteristicType_Id, cascadeDelete: true)
                .ForeignKey("dbo.Skill", t => t.Skill_Id)
                .ForeignKey("dbo.Skill", t => t.Skill_Id1, cascadeDelete: true)
                .ForeignKey("dbo.ThingSample", t => t.ThingSample_Id)
                .ForeignKey("dbo.ThingSample", t => t.ThingSample_Id1)
                .ForeignKey("dbo.Hero", t => t.Hero_Id, cascadeDelete: true)
                .Index(t => t.StateType_Id)
                .Index(t => t.CharacteristicType_Id)
                .Index(t => t.Skill_Id)
                .Index(t => t.Skill_Id1)
                .Index(t => t.ThingSample_Id)
                .Index(t => t.ThingSample_Id1)
                .Index(t => t.Hero_Id);

            CreateTable(
                "dbo.StateType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 120),
                        Desc = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);

            CreateTable(
                "dbo.EventLinkItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        From_Id = c.Long(),
                        To_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Event", t => t.From_Id)
                .ForeignKey("dbo.Event", t => t.To_Id)
                .Index(t => t.From_Id)
                .Index(t => t.To_Id);

            CreateTable(
                "dbo.Characteristic",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Number = c.Long(nullable: false),
                        CharacteristicType_Id = c.Long(),
                        Hero_Id = c.Long(),
                        ThingSample_Id = c.Long(),
                        ThingSample_Id1 = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CharacteristicType", t => t.CharacteristicType_Id)
                .ForeignKey("dbo.Hero", t => t.Hero_Id, cascadeDelete: true)
                .ForeignKey("dbo.ThingSample", t => t.ThingSample_Id)
                .ForeignKey("dbo.ThingSample", t => t.ThingSample_Id1)
                .Index(t => t.CharacteristicType_Id)
                .Index(t => t.Hero_Id)
                .Index(t => t.ThingSample_Id)
                .Index(t => t.ThingSample_Id1);

            CreateTable(
                "dbo.CharacteristicType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 120),
                        Desc = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);

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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SkillSchool", t => t.School_Id)
                .Index(t => t.Name, unique: true)
                .Index(t => t.School_Id);

            CreateTable(
                "dbo.SkillSchool",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 120),
                        Desc = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);

            CreateTable(
                "dbo.Hero",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Background = c.String(),
                        Race = c.Int(nullable: false),
                        Sex = c.Int(nullable: false),
                        Guild_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Guild", t => t.Guild_Id, cascadeDelete: true)
                .Index(t => t.Guild_Id);

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
                "dbo.TrainingRoom",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 120),
                        Price = c.Long(nullable: false),
                        School_Id = c.Long(),
                        Guild_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SkillSchool", t => t.School_Id)
                .ForeignKey("dbo.Guild", t => t.Guild_Id)
                .Index(t => t.Name, unique: true)
                .Index(t => t.School_Id)
                .Index(t => t.Guild_Id);

            CreateTable(
                "dbo.Thing",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ItemInUse = c.Boolean(nullable: false),
                        Count = c.Int(nullable: false),
                        ThingSample_Id = c.Long(),
                        Hero_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ThingSample", t => t.ThingSample_Id)
                .ForeignKey("dbo.Hero", t => t.Hero_Id)
                .Index(t => t.ThingSample_Id)
                .Index(t => t.Hero_Id);

            CreateTable(
                "dbo.ThingSample",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Desc = c.String(),
                        RequrmentSex = c.Int(nullable: false),
                        RequrmentRace = c.Int(nullable: false),
                        IsUsed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.EventState",
                c => new
                    {
                        Event_Id = c.Long(nullable: false),
                        State_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Event_Id, t.State_Id })
                .ForeignKey("dbo.Event", t => t.Event_Id, cascadeDelete: true)
                .ForeignKey("dbo.State", t => t.State_Id, cascadeDelete: true)
                .Index(t => t.Event_Id)
                .Index(t => t.State_Id);

            CreateTable(
                "dbo.EventCharacteristic",
                c => new
                    {
                        Event_Id = c.Long(nullable: false),
                        Characteristic_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Event_Id, t.Characteristic_Id })
                .ForeignKey("dbo.Event", t => t.Event_Id, cascadeDelete: true)
                .ForeignKey("dbo.Characteristic", t => t.Characteristic_Id, cascadeDelete: true)
                .Index(t => t.Event_Id)
                .Index(t => t.Characteristic_Id);

            CreateTable(
                "dbo.EventSkill",
                c => new
                    {
                        Event_Id = c.Long(nullable: false),
                        Skill_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Event_Id, t.Skill_Id })
                .ForeignKey("dbo.Event", t => t.Event_Id, cascadeDelete: true)
                .ForeignKey("dbo.Skill", t => t.Skill_Id, cascadeDelete: true)
                .Index(t => t.Event_Id)
                .Index(t => t.Skill_Id);

            CreateTable(
                "dbo.HeroSkill",
                c => new
                    {
                        Hero_Id = c.Long(nullable: false),
                        Skill_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Hero_Id, t.Skill_Id })
                .ForeignKey("dbo.Hero", t => t.Hero_Id, cascadeDelete: true)
                .ForeignKey("dbo.Skill", t => t.Skill_Id, cascadeDelete: true)
                .Index(t => t.Hero_Id)
                .Index(t => t.Skill_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Event", "ForRootQuest_Id", "dbo.Quest");
            DropForeignKey("dbo.Quest", "Executor_Id", "dbo.Hero");
            DropForeignKey("dbo.State", "Hero_Id", "dbo.Hero");
            DropForeignKey("dbo.HeroSkill", "Skill_Id", "dbo.Skill");
            DropForeignKey("dbo.HeroSkill", "Hero_Id", "dbo.Hero");
            DropForeignKey("dbo.Thing", "Hero_Id", "dbo.Hero");
            DropForeignKey("dbo.Thing", "ThingSample_Id", "dbo.ThingSample");
            DropForeignKey("dbo.State", "ThingSample_Id1", "dbo.ThingSample");
            DropForeignKey("dbo.Characteristic", "ThingSample_Id1", "dbo.ThingSample");
            DropForeignKey("dbo.State", "ThingSample_Id", "dbo.ThingSample");
            DropForeignKey("dbo.Characteristic", "ThingSample_Id", "dbo.ThingSample");
            DropForeignKey("dbo.TrainingRoom", "Guild_Id", "dbo.Guild");
            DropForeignKey("dbo.TrainingRoom", "School_Id", "dbo.SkillSchool");
            DropForeignKey("dbo.Hero", "Guild_Id", "dbo.Guild");
            DropForeignKey("dbo.Characteristic", "Hero_Id", "dbo.Hero");
            DropForeignKey("dbo.Event", "Quest_Id", "dbo.Quest");
            DropForeignKey("dbo.EventSkill", "Skill_Id", "dbo.Skill");
            DropForeignKey("dbo.EventSkill", "Event_Id", "dbo.Event");
            DropForeignKey("dbo.State", "Skill_Id1", "dbo.Skill");
            DropForeignKey("dbo.State", "Skill_Id", "dbo.Skill");
            DropForeignKey("dbo.Skill", "School_Id", "dbo.SkillSchool");
            DropForeignKey("dbo.EventCharacteristic", "Characteristic_Id", "dbo.Characteristic");
            DropForeignKey("dbo.EventCharacteristic", "Event_Id", "dbo.Event");
            DropForeignKey("dbo.Characteristic", "CharacteristicType_Id", "dbo.CharacteristicType");
            DropForeignKey("dbo.State", "CharacteristicType_Id", "dbo.CharacteristicType");
            DropForeignKey("dbo.EventLinkItem", "To_Id", "dbo.Event");
            DropForeignKey("dbo.EventLinkItem", "From_Id", "dbo.Event");
            DropForeignKey("dbo.EventState", "State_Id", "dbo.State");
            DropForeignKey("dbo.EventState", "Event_Id", "dbo.Event");
            DropForeignKey("dbo.State", "StateType_Id", "dbo.StateType");
            DropIndex("dbo.HeroSkill", new[] { "Skill_Id" });
            DropIndex("dbo.HeroSkill", new[] { "Hero_Id" });
            DropIndex("dbo.EventSkill", new[] { "Skill_Id" });
            DropIndex("dbo.EventSkill", new[] { "Event_Id" });
            DropIndex("dbo.EventCharacteristic", new[] { "Characteristic_Id" });
            DropIndex("dbo.EventCharacteristic", new[] { "Event_Id" });
            DropIndex("dbo.EventState", new[] { "State_Id" });
            DropIndex("dbo.EventState", new[] { "Event_Id" });
            DropIndex("dbo.Thing", new[] { "Hero_Id" });
            DropIndex("dbo.Thing", new[] { "ThingSample_Id" });
            DropIndex("dbo.TrainingRoom", new[] { "Guild_Id" });
            DropIndex("dbo.TrainingRoom", new[] { "School_Id" });
            DropIndex("dbo.TrainingRoom", new[] { "Name" });
            DropIndex("dbo.Hero", new[] { "Guild_Id" });
            DropIndex("dbo.SkillSchool", new[] { "Name" });
            DropIndex("dbo.Skill", new[] { "School_Id" });
            DropIndex("dbo.Skill", new[] { "Name" });
            DropIndex("dbo.CharacteristicType", new[] { "Name" });
            DropIndex("dbo.Characteristic", new[] { "ThingSample_Id1" });
            DropIndex("dbo.Characteristic", new[] { "ThingSample_Id" });
            DropIndex("dbo.Characteristic", new[] { "Hero_Id" });
            DropIndex("dbo.Characteristic", new[] { "CharacteristicType_Id" });
            DropIndex("dbo.EventLinkItem", new[] { "To_Id" });
            DropIndex("dbo.EventLinkItem", new[] { "From_Id" });
            DropIndex("dbo.StateType", new[] { "Name" });
            DropIndex("dbo.State", new[] { "Hero_Id" });
            DropIndex("dbo.State", new[] { "ThingSample_Id1" });
            DropIndex("dbo.State", new[] { "ThingSample_Id" });
            DropIndex("dbo.State", new[] { "Skill_Id1" });
            DropIndex("dbo.State", new[] { "Skill_Id" });
            DropIndex("dbo.State", new[] { "CharacteristicType_Id" });
            DropIndex("dbo.State", new[] { "StateType_Id" });
            DropIndex("dbo.Event", new[] { "ForRootQuest_Id" });
            DropIndex("dbo.Event", new[] { "Quest_Id" });
            DropIndex("dbo.Quest", new[] { "Executor_Id" });
            DropTable("dbo.HeroSkill");
            DropTable("dbo.EventSkill");
            DropTable("dbo.EventCharacteristic");
            DropTable("dbo.EventState");
            DropTable("dbo.ThingSample");
            DropTable("dbo.Thing");
            DropTable("dbo.TrainingRoom");
            DropTable("dbo.Guild");
            DropTable("dbo.Hero");
            DropTable("dbo.SkillSchool");
            DropTable("dbo.Skill");
            DropTable("dbo.CharacteristicType");
            DropTable("dbo.Characteristic");
            DropTable("dbo.EventLinkItem");
            DropTable("dbo.StateType");
            DropTable("dbo.State");
            DropTable("dbo.Event");
            DropTable("dbo.Quest");
        }
    }
}
