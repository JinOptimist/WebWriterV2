namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 120),
                        Email = c.String(),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            AddColumn("dbo.Quest", "Owner_Id", c => c.Long());
            AddColumn("dbo.Hero", "CurrentEvent_Id", c => c.Long());
            CreateIndex("dbo.Quest", "Owner_Id");
            CreateIndex("dbo.Hero", "CurrentEvent_Id");
            AddForeignKey("dbo.Hero", "CurrentEvent_Id", "dbo.Event", "Id");
            AddForeignKey("dbo.Quest", "Owner_Id", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Quest", "Owner_Id", "dbo.User");
            DropForeignKey("dbo.Hero", "CurrentEvent_Id", "dbo.Event");
            DropIndex("dbo.Hero", new[] { "CurrentEvent_Id" });
            DropIndex("dbo.Quest", new[] { "Owner_Id" });
            DropIndex("dbo.User", new[] { "Name" });
            DropColumn("dbo.Hero", "CurrentEvent_Id");
            DropColumn("dbo.Quest", "Owner_Id");
            DropTable("dbo.User");
        }
    }
}
