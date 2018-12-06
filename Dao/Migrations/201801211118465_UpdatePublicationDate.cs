namespace Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePublicationDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Book", "PublicationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Book", "PublicationDate", c => c.DateTime(nullable: false));
        }
    }
}
