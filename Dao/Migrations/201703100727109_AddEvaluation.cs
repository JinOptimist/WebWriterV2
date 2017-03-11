namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddEvaluation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Evaluation",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Mark = c.Long(nullable: false),
                        Comment = c.String(),
                        Created = c.DateTime(nullable: false),
                        Quest_Id = c.Long(),
                        Owner_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quest", t => t.Quest_Id)
                .ForeignKey("dbo.User", t => t.Owner_Id)
                .Index(t => t.Quest_Id)
                .Index(t => t.Owner_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Evaluation", "Owner_Id", "dbo.User");
            DropForeignKey("dbo.Evaluation", "Quest_Id", "dbo.Quest");
            DropIndex("dbo.Evaluation", new[] { "Owner_Id" });
            DropIndex("dbo.Evaluation", new[] { "Quest_Id" });
            DropTable("dbo.Evaluation");
        }
    }
}
