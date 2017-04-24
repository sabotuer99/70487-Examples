namespace CustomConventionsConsole.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialmigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClanName = c.String(),
                        DateModified = c.DateTime(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ninjas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ServedInOniwaban = c.Boolean(nullable: false),
                        ClanId = c.Int(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clans", t => t.ClanId, cascadeDelete: true)
                .Index(t => t.ClanId);
            
            CreateTable(
                "dbo.NinjaEquipments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        NinjaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ninjas", t => t.NinjaId, cascadeDelete: true)
                .Index(t => t.NinjaId, name: "IX_Ninja_Id");
            
            CreateStoredProcedure(
                "dbo.InsertNinja",
                p => new
                    {
                        Name = p.String(),
                        ServedInOniwaban = p.Boolean(),
                        ClanId = p.Int(),
                        DateOfBirth = p.DateTime(),
                        DateModified = p.DateTime(),
                        DateCreated = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Ninjas]([Name], [ServedInOniwaban], [ClanId], [DateOfBirth], [DateModified], [DateCreated])
                      VALUES (@Name, @ServedInOniwaban, @ClanId, @DateOfBirth, @DateModified, @DateCreated)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Ninjas]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Ninjas] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Ninja_Update",
                p => new
                    {
                        Id = p.Int(),
                        Name = p.String(),
                        ServedInOniwaban = p.Boolean(),
                        ClanId = p.Int(),
                        DateOfBirth = p.DateTime(),
                        DateModified = p.DateTime(),
                        DateCreated = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Ninjas]
                      SET [Name] = @Name, [ServedInOniwaban] = @ServedInOniwaban, [ClanId] = @ClanId, [DateOfBirth] = @DateOfBirth, [DateModified] = @DateModified, [DateCreated] = @DateCreated
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Ninja_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Ninjas]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Ninja_Delete");
            DropStoredProcedure("dbo.Ninja_Update");
            DropStoredProcedure("dbo.InsertNinja");
            DropForeignKey("dbo.NinjaEquipments", "NinjaId", "dbo.Ninjas");
            DropForeignKey("dbo.Ninjas", "ClanId", "dbo.Clans");
            DropIndex("dbo.NinjaEquipments", "IX_Ninja_Id");
            DropIndex("dbo.Ninjas", new[] { "ClanId" });
            DropTable("dbo.NinjaEquipments");
            DropTable("dbo.Ninjas");
            DropTable("dbo.Clans");
        }
    }
}
