namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddBookmark : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Hero", "Owner_Id", c => c.Long());
            CreateIndex("dbo.Hero", "Owner_Id");
            AddForeignKey("dbo.Hero", "Owner_Id", "dbo.User", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Hero", "Owner_Id", "dbo.User");
            DropIndex("dbo.Hero", new[] { "Owner_Id" });
            DropColumn("dbo.Hero", "Owner_Id");
        }
    }
}
