namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddQuestForEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Event", "Quest_Id1", c => c.Long());
            CreateIndex("dbo.Event", "Quest_Id1");
            AddForeignKey("dbo.Event", "Quest_Id1", "dbo.Quest", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Event", "Quest_Id1", "dbo.Quest");
            DropIndex("dbo.Event", new[] { "Quest_Id1" });
            DropColumn("dbo.Event", "Quest_Id1");
        }
    }
}
