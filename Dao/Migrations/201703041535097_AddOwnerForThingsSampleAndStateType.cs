namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOwnerForThingsSampleAndStateType : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.StateType", new[] { "Name" });
            AddColumn("dbo.StateType", "Owner_Id", c => c.Long());
            AddColumn("dbo.ThingSample", "Owner_Id", c => c.Long());
            CreateIndex("dbo.StateType", "Name");
            CreateIndex("dbo.StateType", "Owner_Id");
            CreateIndex("dbo.ThingSample", "Owner_Id");
            AddForeignKey("dbo.StateType", "Owner_Id", "dbo.User", "Id");
            AddForeignKey("dbo.ThingSample", "Owner_Id", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ThingSample", "Owner_Id", "dbo.User");
            DropForeignKey("dbo.StateType", "Owner_Id", "dbo.User");
            DropIndex("dbo.ThingSample", new[] { "Owner_Id" });
            DropIndex("dbo.StateType", new[] { "Owner_Id" });
            DropIndex("dbo.StateType", new[] { "Name" });
            DropColumn("dbo.ThingSample", "Owner_Id");
            DropColumn("dbo.StateType", "Owner_Id");
            CreateIndex("dbo.StateType", "Name", unique: true);
        }
    }
}
