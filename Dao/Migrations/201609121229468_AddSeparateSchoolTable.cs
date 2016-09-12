namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddSeparateSchoolTable : DbMigration
    {
        public override void Up()
        {
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

            AddColumn("dbo.Skill", "School_Id", c => c.Long());
            AddColumn("dbo.TrainingRoom", "School_Id", c => c.Long());
            CreateIndex("dbo.Skill", "School_Id");
            CreateIndex("dbo.TrainingRoom", "School_Id");
            AddForeignKey("dbo.Skill", "School_Id", "dbo.SkillSchool", "Id");
            AddForeignKey("dbo.TrainingRoom", "School_Id", "dbo.SkillSchool", "Id");
            DropColumn("dbo.Skill", "School");
            DropColumn("dbo.TrainingRoom", "School");
        }

        public override void Down()
        {
            AddColumn("dbo.TrainingRoom", "School", c => c.Int(nullable: false));
            AddColumn("dbo.Skill", "School", c => c.Int(nullable: false));
            DropForeignKey("dbo.TrainingRoom", "School_Id", "dbo.SkillSchool");
            DropForeignKey("dbo.Skill", "School_Id", "dbo.SkillSchool");
            DropIndex("dbo.TrainingRoom", new[] { "School_Id" });
            DropIndex("dbo.SkillSchool", new[] { "Name" });
            DropIndex("dbo.Skill", new[] { "School_Id" });
            DropColumn("dbo.TrainingRoom", "School_Id");
            DropColumn("dbo.Skill", "School_Id");
            DropTable("dbo.SkillSchool");
        }
    }
}
