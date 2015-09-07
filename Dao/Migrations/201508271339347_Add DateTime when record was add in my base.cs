namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateTimewhenrecordwasaddinmybase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserFromVk", "AddedToMyBase", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserFromVk", "AddedToMyBase");
        }
    }
}
