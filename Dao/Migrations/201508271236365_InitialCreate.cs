namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserFromVk",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        VkId = c.Long(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Sex = c.Int(nullable: false),
                        Nickname = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.FriendId",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FriendsId = c.Long(nullable: false),
                        UserFromVk_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserFromVk", t => t.UserFromVk_Id, cascadeDelete: true)
                .Index(t => t.UserFromVk_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FriendId", "UserFromVk_Id", "dbo.UserFromVk");
            DropIndex("dbo.FriendId", new[] { "UserFromVk_Id" });
            DropTable("dbo.FriendId");
            DropTable("dbo.UserFromVk");
        }
    }
}
