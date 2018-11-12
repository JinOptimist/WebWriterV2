namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderForAnswer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionAnswer", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionAnswer", "Order");
        }
    }
}
