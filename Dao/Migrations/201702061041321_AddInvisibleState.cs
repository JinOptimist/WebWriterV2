namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddInvisibleState : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.State", "Invisible", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.State", "Invisible");
        }
    }
}
