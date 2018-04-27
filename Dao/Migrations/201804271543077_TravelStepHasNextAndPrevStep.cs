namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TravelStepHasNextAndPrevStep : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TravelStep", "PrevStep_Id", c => c.Long());
            CreateIndex("dbo.TravelStep", "PrevStep_Id");
            AddForeignKey("dbo.TravelStep", "PrevStep_Id", "dbo.TravelStep", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TravelStep", "PrevStep_Id", "dbo.TravelStep");
            DropIndex("dbo.TravelStep", new[] { "PrevStep_Id" });
            DropColumn("dbo.TravelStep", "PrevStep_Id");
        }
    }
}
