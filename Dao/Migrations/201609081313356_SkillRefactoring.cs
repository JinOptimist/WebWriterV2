namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SkillRefactoring : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Skill", "Hero_Id", "dbo.Hero");
            DropForeignKey("dbo.State", "Hero_Id", "dbo.Hero");
            DropForeignKey("dbo.Skill", "Event_Id", "dbo.Event");
            DropIndex("dbo.Skill", new[] { "Hero_Id" });
            DropIndex("dbo.Skill", new[] { "Event_Id" });
            DropIndex("dbo.State", new[] { "Hero_Id" });
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

            CreateTable(
                "dbo.HeroState",
                c => new
                    {
                        Hero_Id = c.Long(nullable: false),
                        State_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Hero_Id, t.State_Id })
                .ForeignKey("dbo.Hero", t => t.Hero_Id, cascadeDelete: true)
                .ForeignKey("dbo.State", t => t.State_Id, cascadeDelete: true)
                .Index(t => t.Hero_Id)
                .Index(t => t.State_Id);

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

            AlterColumn("dbo.Skill", "Name", c => c.String(nullable: false, maxLength: 120));
            CreateIndex("dbo.Skill", "Name", unique: true);
            DropColumn("dbo.Skill", "Hero_Id");
            DropColumn("dbo.Skill", "Event_Id");
            DropColumn("dbo.State", "Hero_Id");
        }

        public override void Down()
        {
            AddColumn("dbo.State", "Hero_Id", c => c.Long());
            AddColumn("dbo.Skill", "Event_Id", c => c.Long());
            AddColumn("dbo.Skill", "Hero_Id", c => c.Long());
            DropForeignKey("dbo.EventSkill", "Skill_Id", "dbo.Skill");
            DropForeignKey("dbo.EventSkill", "Event_Id", "dbo.Event");
            DropForeignKey("dbo.HeroState", "State_Id", "dbo.State");
            DropForeignKey("dbo.HeroState", "Hero_Id", "dbo.Hero");
            DropForeignKey("dbo.HeroSkill", "Skill_Id", "dbo.Skill");
            DropForeignKey("dbo.HeroSkill", "Hero_Id", "dbo.Hero");
            DropIndex("dbo.EventSkill", new[] { "Skill_Id" });
            DropIndex("dbo.EventSkill", new[] { "Event_Id" });
            DropIndex("dbo.HeroState", new[] { "State_Id" });
            DropIndex("dbo.HeroState", new[] { "Hero_Id" });
            DropIndex("dbo.HeroSkill", new[] { "Skill_Id" });
            DropIndex("dbo.HeroSkill", new[] { "Hero_Id" });
            DropIndex("dbo.Skill", new[] { "Name" });
            AlterColumn("dbo.Skill", "Name", c => c.String(nullable: false));
            DropTable("dbo.EventSkill");
            DropTable("dbo.HeroState");
            DropTable("dbo.HeroSkill");
            CreateIndex("dbo.State", "Hero_Id");
            CreateIndex("dbo.Skill", "Event_Id");
            CreateIndex("dbo.Skill", "Hero_Id");
            AddForeignKey("dbo.Skill", "Event_Id", "dbo.Event", "Id");
            AddForeignKey("dbo.State", "Hero_Id", "dbo.Hero", "Id");
            AddForeignKey("dbo.Skill", "Hero_Id", "dbo.Hero", "Id");
        }
    }
}
