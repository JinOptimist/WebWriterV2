namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQuestionnaire : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionnaireResult",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Questionnaire_Id = c.Long(nullable: false),
                        User_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questionnaire", t => t.Questionnaire_Id, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.Questionnaire_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.QuestionAnswer",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Text = c.String(),
                        Question_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Question", t => t.Question_Id)
                .Index(t => t.Question_Id);
            
            CreateTable(
                "dbo.Question",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Text = c.String(),
                        AllowMultipleAnswers = c.Boolean(nullable: false),
                        Order = c.Int(nullable: false),
                        Questionnaire_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questionnaire", t => t.Questionnaire_Id, cascadeDelete: true)
                .Index(t => t.Questionnaire_Id);
            
            CreateTable(
                "dbo.Questionnaire",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuestionnaireUser",
                c => new
                    {
                        Questionnaire_Id = c.Long(nullable: false),
                        User_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Questionnaire_Id, t.User_Id })
                .ForeignKey("dbo.Questionnaire", t => t.Questionnaire_Id, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Questionnaire_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.QuestionQuestionAnswer",
                c => new
                    {
                        Question_Id = c.Long(nullable: false),
                        QuestionAnswer_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Question_Id, t.QuestionAnswer_Id })
                .ForeignKey("dbo.Question", t => t.Question_Id, cascadeDelete: true)
                .ForeignKey("dbo.QuestionAnswer", t => t.QuestionAnswer_Id, cascadeDelete: true)
                .Index(t => t.Question_Id)
                .Index(t => t.QuestionAnswer_Id);
            
            CreateTable(
                "dbo.QuestionnaireResultQuestionAnswer",
                c => new
                    {
                        QuestionnaireResult_Id = c.Long(nullable: false),
                        QuestionAnswer_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.QuestionnaireResult_Id, t.QuestionAnswer_Id })
                .ForeignKey("dbo.QuestionnaireResult", t => t.QuestionnaireResult_Id, cascadeDelete: true)
                .ForeignKey("dbo.QuestionAnswer", t => t.QuestionAnswer_Id, cascadeDelete: true)
                .Index(t => t.QuestionnaireResult_Id)
                .Index(t => t.QuestionAnswer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionnaireResult", "User_Id", "dbo.User");
            DropForeignKey("dbo.QuestionnaireResultQuestionAnswer", "QuestionAnswer_Id", "dbo.QuestionAnswer");
            DropForeignKey("dbo.QuestionnaireResultQuestionAnswer", "QuestionnaireResult_Id", "dbo.QuestionnaireResult");
            DropForeignKey("dbo.QuestionQuestionAnswer", "QuestionAnswer_Id", "dbo.QuestionAnswer");
            DropForeignKey("dbo.QuestionQuestionAnswer", "Question_Id", "dbo.Question");
            DropForeignKey("dbo.QuestionnaireUser", "User_Id", "dbo.User");
            DropForeignKey("dbo.QuestionnaireUser", "Questionnaire_Id", "dbo.Questionnaire");
            DropForeignKey("dbo.Question", "Questionnaire_Id", "dbo.Questionnaire");
            DropForeignKey("dbo.QuestionnaireResult", "Questionnaire_Id", "dbo.Questionnaire");
            DropForeignKey("dbo.QuestionAnswer", "Question_Id", "dbo.Question");
            DropIndex("dbo.QuestionnaireResultQuestionAnswer", new[] { "QuestionAnswer_Id" });
            DropIndex("dbo.QuestionnaireResultQuestionAnswer", new[] { "QuestionnaireResult_Id" });
            DropIndex("dbo.QuestionQuestionAnswer", new[] { "QuestionAnswer_Id" });
            DropIndex("dbo.QuestionQuestionAnswer", new[] { "Question_Id" });
            DropIndex("dbo.QuestionnaireUser", new[] { "User_Id" });
            DropIndex("dbo.QuestionnaireUser", new[] { "Questionnaire_Id" });
            DropIndex("dbo.Question", new[] { "Questionnaire_Id" });
            DropIndex("dbo.QuestionAnswer", new[] { "Question_Id" });
            DropIndex("dbo.QuestionnaireResult", new[] { "User_Id" });
            DropIndex("dbo.QuestionnaireResult", new[] { "Questionnaire_Id" });
            DropTable("dbo.QuestionnaireResultQuestionAnswer");
            DropTable("dbo.QuestionQuestionAnswer");
            DropTable("dbo.QuestionnaireUser");
            DropTable("dbo.Questionnaire");
            DropTable("dbo.Question");
            DropTable("dbo.QuestionAnswer");
            DropTable("dbo.QuestionnaireResult");
        }
    }
}
