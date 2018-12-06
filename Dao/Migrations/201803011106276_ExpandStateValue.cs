namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExpandStateValue : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.StateChange", name: "Chapter_Id", newName: "ChapterLink_Id");
            RenameIndex(table: "dbo.StateChange", name: "IX_Chapter_Id", newName: "IX_ChapterLink_Id");
            AddColumn("dbo.StateChange", "Text", c => c.String());
            AddColumn("dbo.StateValue", "Text", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StateValue", "Text");
            DropColumn("dbo.StateChange", "Text");
            RenameIndex(table: "dbo.StateChange", name: "IX_ChapterLink_Id", newName: "IX_Chapter_Id");
            RenameColumn(table: "dbo.StateChange", name: "ChapterLink_Id", newName: "Chapter_Id");
        }
    }
}
