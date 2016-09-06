namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddSkills : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Thing", "Owner_Id", "dbo.Hero");
            DropForeignKey("dbo.Hero", "Location_Id", "dbo.Location");
            DropIndex("dbo.Hero", new[] { "Location_Id" });
            DropIndex("dbo.Thing", new[] { "Owner_Id" });
            AddColumn("dbo.Skill", "Hero_Id", c => c.Long());
            AlterColumn("dbo.Hero", "Location_Id", c => c.Long());
            AlterColumn("dbo.Location", "Desc", c => c.String());
            CreateIndex("dbo.Hero", "Location_Id");
            CreateIndex("dbo.Skill", "Hero_Id");
            AddForeignKey("dbo.Skill", "Hero_Id", "dbo.Hero", "Id");
            AddForeignKey("dbo.Hero", "Location_Id", "dbo.Location", "Id");
            DropTable("dbo.Thing");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.Thing",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Owner_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id);

            DropForeignKey("dbo.Hero", "Location_Id", "dbo.Location");
            DropForeignKey("dbo.Skill", "Hero_Id", "dbo.Hero");
            DropIndex("dbo.Skill", new[] { "Hero_Id" });
            DropIndex("dbo.Hero", new[] { "Location_Id" });
            AlterColumn("dbo.Location", "Desc", c => c.String(nullable: false));
            AlterColumn("dbo.Hero", "Location_Id", c => c.Long(nullable: false));
            DropColumn("dbo.Skill", "Hero_Id");
            CreateIndex("dbo.Thing", "Owner_Id");
            CreateIndex("dbo.Hero", "Location_Id");
            AddForeignKey("dbo.Hero", "Location_Id", "dbo.Location", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Thing", "Owner_Id", "dbo.Hero", "Id");
        }
    }
}
