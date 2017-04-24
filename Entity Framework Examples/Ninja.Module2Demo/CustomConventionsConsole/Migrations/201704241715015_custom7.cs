namespace CustomConventionsConsole.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class custom7 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Clanzz", newName: "Clans");
            RenameTable(name: "dbo.Ninjazz", newName: "Ninjas");
            RenameTable(name: "dbo.NinjaEquipmentzz", newName: "NinjaEquipments");
            RenameColumn(table: "dbo.Ninjas", name: "IsBadass", newName: "ServedInOniwaban");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Ninjas", name: "ServedInOniwaban", newName: "IsBadass");
            RenameTable(name: "dbo.NinjaEquipments", newName: "NinjaEquipmentzz");
            RenameTable(name: "dbo.Ninjas", newName: "Ninjazz");
            RenameTable(name: "dbo.Clans", newName: "Clanzz");
        }
    }
}
