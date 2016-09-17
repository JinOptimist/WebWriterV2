namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateHeroStat : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Characteristic", "Hero_Id", "dbo.Hero");
            AddForeignKey("dbo.Characteristic", "Hero_Id", "dbo.Hero", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Characteristic", "Hero_Id", "dbo.Hero");
            AddForeignKey("dbo.Characteristic", "Hero_Id", "dbo.Hero", "Id");
        }
    }
}
