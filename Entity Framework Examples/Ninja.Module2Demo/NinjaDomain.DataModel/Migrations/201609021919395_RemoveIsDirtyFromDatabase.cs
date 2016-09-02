namespace NinjaDomain.DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIsDirtyFromDatabase : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Clans", "IsDirty");
            DropColumn("dbo.Ninjas", "IsDirty");
            DropColumn("dbo.NinjaEquipments", "IsDirty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NinjaEquipments", "IsDirty", c => c.Boolean(nullable: false));
            AddColumn("dbo.Ninjas", "IsDirty", c => c.Boolean(nullable: false));
            AddColumn("dbo.Clans", "IsDirty", c => c.Boolean(nullable: false));
        }
    }
}
