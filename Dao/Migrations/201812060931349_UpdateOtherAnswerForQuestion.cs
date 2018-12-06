namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOtherAnswerForQuestion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionOtherAnswer",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AnswerText = c.String(),
                        Question_Id = c.Long(),
                        QuestionnaireResult_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Question", t => t.Question_Id)
                .ForeignKey("dbo.QuestionnaireResult", t => t.QuestionnaireResult_Id, cascadeDelete: true)
                .Index(t => t.Question_Id)
                .Index(t => t.QuestionnaireResult_Id);
            
            DropColumn("dbo.Question", "OtherAnswer");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Question", "OtherAnswer", c => c.String());
            DropForeignKey("dbo.QuestionOtherAnswer", "QuestionnaireResult_Id", "dbo.QuestionnaireResult");
            DropForeignKey("dbo.QuestionOtherAnswer", "Question_Id", "dbo.Question");
            DropIndex("dbo.QuestionOtherAnswer", new[] { "QuestionnaireResult_Id" });
            DropIndex("dbo.QuestionOtherAnswer", new[] { "Question_Id" });
            DropTable("dbo.QuestionOtherAnswer");
        }
    }
}
