namespace TablePerType.Migrations
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
                    })
                .PrimaryKey(t => t.BillingDetailId);
            
            CreateTable(
                "dbo.BankAccounts",
                c => new
                    {
                        BillingDetailId = c.Int(nullable: false),
                        BankName = c.String(),
                        Swift = c.String(),
                    })
                .PrimaryKey(t => t.BillingDetailId)
                .ForeignKey("dbo.BillingDetails", t => t.BillingDetailId)
                .Index(t => t.BillingDetailId);
            
            CreateTable(
                "dbo.CreditCards",
                c => new
                    {
                        BillingDetailId = c.Int(nullable: false),
                        CardType = c.Int(nullable: false),
                        ExpiryMonth = c.String(),
                        ExpiryYear = c.String(),
                    })
                .PrimaryKey(t => t.BillingDetailId)
                .ForeignKey("dbo.BillingDetails", t => t.BillingDetailId)
                .Index(t => t.BillingDetailId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CreditCards", "BillingDetailId", "dbo.BillingDetails");
            DropForeignKey("dbo.BankAccounts", "BillingDetailId", "dbo.BillingDetails");
            DropIndex("dbo.CreditCards", new[] { "BillingDetailId" });
            DropIndex("dbo.BankAccounts", new[] { "BillingDetailId" });
            DropTable("dbo.CreditCards");
            DropTable("dbo.BankAccounts");
            DropTable("dbo.BillingDetails");
        }
    }
}
