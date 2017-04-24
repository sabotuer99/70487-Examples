namespace CustomConventionsConsole.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class custom6 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Clans", newName: "Clanzz");
            RenameTable(name: "dbo.Ninjas", newName: "Ninjazz");
            RenameTable(name: "dbo.NinjaEquipments", newName: "NinjaEquipmentzz");
            RenameColumn(table: "dbo.NinjaEquipmentzz", name: "NinjaId", newName: "Ninja_Id");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.NinjaEquipmentzz", name: "Ninja_Id", newName: "NinjaId");
            RenameTable(name: "dbo.NinjaEquipmentzz", newName: "NinjaEquipments");
            RenameTable(name: "dbo.Ninjazz", newName: "Ninjas");
            RenameTable(name: "dbo.Clanzz", newName: "Clans");
        }
    }
}
