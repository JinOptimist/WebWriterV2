namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class travelHasCurrentStep : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Travel", "CurrentChapter_Id", "dbo.Chapter");
            DropIndex("dbo.Travel", new[] { "CurrentChapter_Id" });
            AddColumn("dbo.Travel", "CurrentStep_Id", c => c.Long());
            AddColumn("dbo.TravelStep", "CurrentChapter_Id", c => c.Long());
            CreateIndex("dbo.Travel", "CurrentStep_Id");
            CreateIndex("dbo.TravelStep", "CurrentChapter_Id");
            AddForeignKey("dbo.TravelStep", "CurrentChapter_Id", "dbo.Chapter", "Id");
            AddForeignKey("dbo.Travel", "CurrentStep_Id", "dbo.TravelStep", "Id");
            DropColumn("dbo.Travel", "CurrentChapter_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Travel", "CurrentChapter_Id", c => c.Long());
            DropForeignKey("dbo.Travel", "CurrentStep_Id", "dbo.TravelStep");
            DropForeignKey("dbo.TravelStep", "CurrentChapter_Id", "dbo.Chapter");
            DropIndex("dbo.TravelStep", new[] { "CurrentChapter_Id" });
            DropIndex("dbo.Travel", new[] { "CurrentStep_Id" });
            DropColumn("dbo.TravelStep", "CurrentChapter_Id");
            DropColumn("dbo.Travel", "CurrentStep_Id");
            CreateIndex("dbo.Travel", "CurrentChapter_Id");
            AddForeignKey("dbo.Travel", "CurrentChapter_Id", "dbo.Chapter", "Id");
        }
    }
}
