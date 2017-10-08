namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameEventToChapter : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Event", newName: "Chapter");
            RenameTable(name: "dbo.EventState", newName: "ChapterState");
            RenameTable(name: "dbo.EventState1", newName: "ChapterState1");
            RenameColumn(table: "dbo.ChapterState", name: "Event_Id", newName: "Chapter_Id");
            RenameColumn(table: "dbo.ChapterState1", name: "Event_Id", newName: "Chapter_Id");
            RenameColumn(table: "dbo.Thing", name: "Event_Id", newName: "Chapter_Id");
            RenameColumn(table: "dbo.Thing", name: "Event_Id1", newName: "Chapter_Id1");
            RenameColumn(table: "dbo.Hero", name: "CurrentEvent_Id", newName: "CurrentChapter_Id");
            RenameIndex(table: "dbo.Hero", name: "IX_CurrentEvent_Id", newName: "IX_CurrentChapter_Id");
            RenameIndex(table: "dbo.Thing", name: "IX_Event_Id", newName: "IX_Chapter_Id");
            RenameIndex(table: "dbo.Thing", name: "IX_Event_Id1", newName: "IX_Chapter_Id1");
            RenameIndex(table: "dbo.ChapterState", name: "IX_Event_Id", newName: "IX_Chapter_Id");
            RenameIndex(table: "dbo.ChapterState1", name: "IX_Event_Id", newName: "IX_Chapter_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ChapterState1", name: "IX_Chapter_Id", newName: "IX_Event_Id");
            RenameIndex(table: "dbo.ChapterState", name: "IX_Chapter_Id", newName: "IX_Event_Id");
            RenameIndex(table: "dbo.Thing", name: "IX_Chapter_Id1", newName: "IX_Event_Id1");
            RenameIndex(table: "dbo.Thing", name: "IX_Chapter_Id", newName: "IX_Event_Id");
            RenameIndex(table: "dbo.Hero", name: "IX_CurrentChapter_Id", newName: "IX_CurrentEvent_Id");
            RenameColumn(table: "dbo.Hero", name: "CurrentChapter_Id", newName: "CurrentEvent_Id");
            RenameColumn(table: "dbo.Thing", name: "Chapter_Id1", newName: "Event_Id1");
            RenameColumn(table: "dbo.Thing", name: "Chapter_Id", newName: "Event_Id");
            RenameColumn(table: "dbo.ChapterState1", name: "Chapter_Id", newName: "Event_Id");
            RenameColumn(table: "dbo.ChapterState", name: "Chapter_Id", newName: "Event_Id");
            RenameTable(name: "dbo.ChapterState1", newName: "EventState1");
            RenameTable(name: "dbo.ChapterState", newName: "EventState");
            RenameTable(name: "dbo.Chapter", newName: "Event");
        }
    }
}
