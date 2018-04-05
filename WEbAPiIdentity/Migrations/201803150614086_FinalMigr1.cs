namespace WEbAPiIdentity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinalMigr1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Images", "PID", "dbo.Products");
            DropIndex("dbo.Images", new[] { "PID" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Images", "PID");
            AddForeignKey("dbo.Images", "PID", "dbo.Products", "PID", cascadeDelete: true);
        }
    }
}
