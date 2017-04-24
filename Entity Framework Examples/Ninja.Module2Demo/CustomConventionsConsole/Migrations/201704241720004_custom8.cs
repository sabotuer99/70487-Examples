namespace CustomConventionsConsole.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class custom8 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Clans", newName: "Clanz");
            RenameTable(name: "dbo.Ninjas", newName: "Ninjaz");
            RenameTable(name: "dbo.NinjaEquipments", newName: "NinjaEquipmentz");
            RenameColumn(table: "dbo.Ninjaz", name: "ServedInOniwaban", newName: "IsBadass");
            AlterStoredProcedure(
                "dbo.Ninja_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(),
                        IsBadass = p.Boolean(),
                        ClanId = p.Int(),
                        DateOfBirth = p.DateTime(),
                        DateModified = p.DateTime(),
                        DateCreated = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Ninjaz]
                      SET [Name] = @Name, [IsBadass] = @IsBadass, [ClanId] = @ClanId, [DateOfBirth] = @DateOfBirth, [DateModified] = @DateModified, [DateCreated] = @DateCreated
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Ninjaz", name: "IsBadass", newName: "ServedInOniwaban");
            RenameTable(name: "dbo.NinjaEquipmentz", newName: "NinjaEquipments");
            RenameTable(name: "dbo.Ninjaz", newName: "Ninjas");
            RenameTable(name: "dbo.Clanz", newName: "Clans");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
