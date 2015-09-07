namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHero : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hero",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        FaceImgUrl = c.String(),
                        FullImgUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Hero");
        }
    }
}
