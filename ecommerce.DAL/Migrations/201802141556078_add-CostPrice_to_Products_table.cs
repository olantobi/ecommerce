namespace ecommerce.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCostPrice_to_Products_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "CostPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "CostPrice");
        }
    }
}
