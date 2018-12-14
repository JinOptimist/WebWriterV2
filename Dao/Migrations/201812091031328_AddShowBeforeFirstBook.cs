namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShowBeforeFirstBook : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questionnaire", "ShowBeforeFirstBook", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questionnaire", "ShowBeforeFirstBook");
        }
    }
}
