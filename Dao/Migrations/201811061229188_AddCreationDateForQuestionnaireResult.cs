namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreationDateForQuestionnaireResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionnaireResult", "CreationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionnaireResult", "CreationDate");
        }
    }
}
