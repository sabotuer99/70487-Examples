namespace TablePerClass.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial_db_create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankAccounts",
                c => new
                    {
                        BillingDetailId = c.Int(nullable: false),
                        Owner = c.String(),
                        Number = c.String(),
                        BankName = c.String(),
                        Swift = c.String(),
                    })
                .PrimaryKey(t => t.BillingDetailId);
            
            CreateTable(
                "dbo.CreditCards",
                c => new
                    {
                        BillingDetailId = c.Int(nullable: false),
                        Owner = c.String(),
                        Number = c.String(),
                        CardType = c.Int(nullable: false),
                        ExpiryMonth = c.String(),
                        ExpiryYear = c.String(),
                    })
                .PrimaryKey(t => t.BillingDetailId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CreditCards");
            DropTable("dbo.BankAccounts");
        }
    }
}
