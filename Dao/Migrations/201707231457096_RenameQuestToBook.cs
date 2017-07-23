namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameQuestToBook : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Quest", newName: "Book");
            RenameTable(name: "dbo.UserQuest", newName: "UserBook");
            RenameColumn(table: "dbo.UserBook", name: "Quest_Id", newName: "Book_Id");
            RenameColumn(table: "dbo.Event", name: "Quest_Id", newName: "Book_Id");
            RenameColumn(table: "dbo.Evaluation", name: "Quest_Id", newName: "Book_Id");
            RenameColumn(table: "dbo.Event", name: "ForRootQuest_Id", newName: "ForRootBook_Id");
            RenameIndex(table: "dbo.Event", name: "IX_Quest_Id", newName: "IX_Book_Id");
            RenameIndex(table: "dbo.Event", name: "IX_ForRootQuest_Id", newName: "IX_ForRootBook_Id");
            RenameIndex(table: "dbo.Evaluation", name: "IX_Quest_Id", newName: "IX_Book_Id");
            RenameIndex(table: "dbo.UserBook", name: "IX_Quest_Id", newName: "IX_Book_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.UserBook", name: "IX_Book_Id", newName: "IX_Quest_Id");
            RenameIndex(table: "dbo.Evaluation", name: "IX_Book_Id", newName: "IX_Quest_Id");
            RenameIndex(table: "dbo.Event", name: "IX_ForRootBook_Id", newName: "IX_ForRootQuest_Id");
            RenameIndex(table: "dbo.Event", name: "IX_Book_Id", newName: "IX_Quest_Id");
            RenameColumn(table: "dbo.Event", name: "ForRootBook_Id", newName: "ForRootQuest_Id");
            RenameColumn(table: "dbo.Evaluation", name: "Book_Id", newName: "Quest_Id");
            RenameColumn(table: "dbo.Event", name: "Book_Id", newName: "Quest_Id");
            RenameColumn(table: "dbo.UserBook", name: "Book_Id", newName: "Quest_Id");
            RenameTable(name: "dbo.UserBook", newName: "UserQuest");
            RenameTable(name: "dbo.Book", newName: "Quest");
        }
    }
}
