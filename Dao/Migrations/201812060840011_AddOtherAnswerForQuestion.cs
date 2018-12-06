namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOtherAnswerForQuestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Question", "EnableOtherAnswer", c => c.Boolean(nullable: false));
            AddColumn("dbo.Question", "OtherLabel", c => c.String());
            AddColumn("dbo.Question", "OtherAnswer", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Question", "OtherAnswer");
            DropColumn("dbo.Question", "OtherLabel");
            DropColumn("dbo.Question", "EnableOtherAnswer");
        }
    }
}
