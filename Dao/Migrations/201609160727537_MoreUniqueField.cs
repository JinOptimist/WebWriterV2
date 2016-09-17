namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class MoreUniqueField : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Guild", "Name", c => c.String(nullable: false, maxLength: 120));
            AlterColumn("dbo.CharacteristicType", "Name", c => c.String(nullable: false, maxLength: 120));
            AlterColumn("dbo.StateType", "Name", c => c.String(nullable: false, maxLength: 120));
            AlterColumn("dbo.TrainingRoom", "Name", c => c.String(nullable: false, maxLength: 120));
            CreateIndex("dbo.Guild", "Name", unique: true);
            CreateIndex("dbo.CharacteristicType", "Name", unique: true);
            CreateIndex("dbo.StateType", "Name", unique: true);
            CreateIndex("dbo.TrainingRoom", "Name", unique: true);
        }

        public override void Down()
        {
            DropIndex("dbo.TrainingRoom", new[] { "Name" });
            DropIndex("dbo.StateType", new[] { "Name" });
            DropIndex("dbo.CharacteristicType", new[] { "Name" });
            DropIndex("dbo.Guild", new[] { "Name" });
            AlterColumn("dbo.TrainingRoom", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.StateType", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.CharacteristicType", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Guild", "Name", c => c.String(nullable: false));
        }
    }
}
