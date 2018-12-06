namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCascadeDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TravelStep", "Сhoice_Id", "dbo.ChapterLinkItem");
            AddForeignKey("dbo.TravelStep", "Сhoice_Id", "dbo.ChapterLinkItem", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TravelStep", "Сhoice_Id", "dbo.ChapterLinkItem");
            AddForeignKey("dbo.TravelStep", "Сhoice_Id", "dbo.ChapterLinkItem", "Id");
        }
    }
}
