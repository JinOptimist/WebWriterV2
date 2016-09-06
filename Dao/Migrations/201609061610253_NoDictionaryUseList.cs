namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class NoDictionaryUseList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Characteristic",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Number = c.Long(nullable: false),
                        Hero_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hero", t => t.Hero_Id)
                .Index(t => t.Hero_Id);

            CreateTable(
                "dbo.State",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Number = c.Long(nullable: false),
                        Hero_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hero", t => t.Hero_Id)
                .Index(t => t.Hero_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.State", "Hero_Id", "dbo.Hero");
            DropForeignKey("dbo.Characteristic", "Hero_Id", "dbo.Hero");
            DropIndex("dbo.State", new[] { "Hero_Id" });
            DropIndex("dbo.Characteristic", new[] { "Hero_Id" });
            DropTable("dbo.State");
            DropTable("dbo.Characteristic");
        }
    }
}
