namespace CustomConventionsConsole.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class custom5 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Ninjas", name: "ServedInOniwaban", newName: "IsBadass");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Ninjas", name: "IsBadass", newName: "ServedInOniwaban");
        }
    }
}
