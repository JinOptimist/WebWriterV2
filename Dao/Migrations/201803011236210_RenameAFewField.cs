namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameAFewField : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.StateRequirement", name: "Chapter_Id", newName: "ChapterLink_Id");
            RenameIndex(table: "dbo.StateRequirement", name: "IX_Chapter_Id", newName: "IX_ChapterLink_Id");
            AddColumn("dbo.StateRequirement", "Text", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StateRequirement", "Text");
            RenameIndex(table: "dbo.StateRequirement", name: "IX_ChapterLink_Id", newName: "IX_Chapter_Id");
            RenameColumn(table: "dbo.StateRequirement", name: "ChapterLink_Id", newName: "Chapter_Id");
        }
    }
}
