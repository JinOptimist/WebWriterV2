namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Stabilize : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.State", "ThingSample_Id", "dbo.ThingSample");
            DropForeignKey("dbo.State", "ThingSample_Id1", "dbo.ThingSample");
            AddForeignKey("dbo.State", "ThingSample_Id", "dbo.ThingSample", "Id");
            AddForeignKey("dbo.State", "ThingSample_Id1", "dbo.ThingSample", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.State", "ThingSample_Id1", "dbo.ThingSample");
            DropForeignKey("dbo.State", "ThingSample_Id", "dbo.ThingSample");
            AddForeignKey("dbo.State", "ThingSample_Id1", "dbo.ThingSample", "Id", cascadeDelete: true);
            AddForeignKey("dbo.State", "ThingSample_Id", "dbo.ThingSample", "Id", cascadeDelete: true);
        }
    }
}
