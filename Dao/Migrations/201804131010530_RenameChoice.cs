namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameChoice : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TravelStep", name: "Сhoice_Id", newName: "Choice_Id");
            RenameIndex(table: "dbo.TravelStep", name: "IX_Сhoice_Id", newName: "IX_Choice_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.TravelStep", name: "IX_Choice_Id", newName: "IX_Сhoice_Id");
            RenameColumn(table: "dbo.TravelStep", name: "Choice_Id", newName: "Сhoice_Id");
        }
    }
}
