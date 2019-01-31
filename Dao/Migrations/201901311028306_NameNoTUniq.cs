namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NameNoTUniq : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.User", new[] { "Name" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.User", "Name", unique: true);
        }
    }
}
