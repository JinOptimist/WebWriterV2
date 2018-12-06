namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookHasStates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StateType", "BasicType", c => c.Int(nullable: false));
            AddColumn("dbo.StateType", "Book_Id", c => c.Long());
            CreateIndex("dbo.StateType", "Book_Id");
            AddForeignKey("dbo.StateType", "Book_Id", "dbo.Book", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StateType", "Book_Id", "dbo.Book");
            DropIndex("dbo.StateType", new[] { "Book_Id" });
            DropColumn("dbo.StateType", "Book_Id");
            DropColumn("dbo.StateType", "BasicType");
        }
    }
}
