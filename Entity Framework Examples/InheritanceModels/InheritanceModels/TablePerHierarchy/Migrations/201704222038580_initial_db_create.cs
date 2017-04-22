namespace TablePerHierarchy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial_db_create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BillingDetails",
                c => new
                    {
                        BillingDetailId = c.Int(nullable: false, identity: true),
                        Owner = c.String(),
                        Number = c.String(),
                        BankName = c.String(),
                        Swift = c.String(),
                        CardType = c.Int(),
                        ExpiryMonth = c.String(),
                        ExpiryYear = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.BillingDetailId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BillingDetails");
        }
    }
}
