namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Characteristic", "Thing_Id", "dbo.Thing");
            DropForeignKey("dbo.State", "Thing_Id", "dbo.Thing");
            DropIndex("dbo.State", new[] { "Thing_Id" });
            DropIndex("dbo.Characteristic", new[] { "Thing_Id" });
            RenameColumn(table: "dbo.Thing", name: "Owner_Id", newName: "Hero_Id");
            RenameIndex(table: "dbo.Thing", name: "IX_Owner_Id", newName: "IX_Hero_Id");
            CreateTable(
                "dbo.ThingSample",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Desc = c.String(),
                        IsUsed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.State", "ThingSample_Id", c => c.Long());
            AddColumn("dbo.State", "ThingSample_Id1", c => c.Long());
            AddColumn("dbo.Characteristic", "ThingSample_Id", c => c.Long());
            AddColumn("dbo.Characteristic", "ThingSample_Id1", c => c.Long());
            AddColumn("dbo.Thing", "ItemInUse", c => c.Boolean(nullable: false));
            AddColumn("dbo.Thing", "Count", c => c.Int(nullable: false));
            AddColumn("dbo.Thing", "ThingSample_Id", c => c.Long());
            CreateIndex("dbo.State", "ThingSample_Id");
            CreateIndex("dbo.State", "ThingSample_Id1");
            CreateIndex("dbo.Characteristic", "ThingSample_Id");
            CreateIndex("dbo.Characteristic", "ThingSample_Id1");
            CreateIndex("dbo.Thing", "ThingSample_Id");
            AddForeignKey("dbo.Characteristic", "ThingSample_Id", "dbo.ThingSample", "Id");
            AddForeignKey("dbo.State", "ThingSample_Id", "dbo.ThingSample", "Id");
            AddForeignKey("dbo.Characteristic", "ThingSample_Id1", "dbo.ThingSample", "Id");
            AddForeignKey("dbo.State", "ThingSample_Id1", "dbo.ThingSample", "Id");
            AddForeignKey("dbo.Thing", "ThingSample_Id", "dbo.ThingSample", "Id");
            DropColumn("dbo.State", "Thing_Id");
            DropColumn("dbo.Characteristic", "Thing_Id");
            DropColumn("dbo.Thing", "Name");
            DropColumn("dbo.Thing", "Desc");
            DropColumn("dbo.Thing", "IsUsed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Thing", "IsUsed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Thing", "Desc", c => c.String());
            AddColumn("dbo.Thing", "Name", c => c.String(nullable: false));
            AddColumn("dbo.Characteristic", "Thing_Id", c => c.Long());
            AddColumn("dbo.State", "Thing_Id", c => c.Long());
            DropForeignKey("dbo.Thing", "ThingSample_Id", "dbo.ThingSample");
            DropForeignKey("dbo.State", "ThingSample_Id1", "dbo.ThingSample");
            DropForeignKey("dbo.Characteristic", "ThingSample_Id1", "dbo.ThingSample");
            DropForeignKey("dbo.State", "ThingSample_Id", "dbo.ThingSample");
            DropForeignKey("dbo.Characteristic", "ThingSample_Id", "dbo.ThingSample");
            DropIndex("dbo.Thing", new[] { "ThingSample_Id" });
            DropIndex("dbo.Characteristic", new[] { "ThingSample_Id1" });
            DropIndex("dbo.Characteristic", new[] { "ThingSample_Id" });
            DropIndex("dbo.State", new[] { "ThingSample_Id1" });
            DropIndex("dbo.State", new[] { "ThingSample_Id" });
            DropColumn("dbo.Thing", "ThingSample_Id");
            DropColumn("dbo.Thing", "Count");
            DropColumn("dbo.Thing", "ItemInUse");
            DropColumn("dbo.Characteristic", "ThingSample_Id1");
            DropColumn("dbo.Characteristic", "ThingSample_Id");
            DropColumn("dbo.State", "ThingSample_Id1");
            DropColumn("dbo.State", "ThingSample_Id");
            DropTable("dbo.ThingSample");
            RenameIndex(table: "dbo.Thing", name: "IX_Hero_Id", newName: "IX_Owner_Id");
            RenameColumn(table: "dbo.Thing", name: "Hero_Id", newName: "Owner_Id");
            CreateIndex("dbo.Characteristic", "Thing_Id");
            CreateIndex("dbo.State", "Thing_Id");
            AddForeignKey("dbo.State", "Thing_Id", "dbo.Thing", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Characteristic", "Thing_Id", "dbo.Thing", "Id", cascadeDelete: true);
        }
    }
}
