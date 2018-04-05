namespace WEbAPiIdentity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "imageId", c => c.Int(nullable: false));
            DropColumn("dbo.Images", "PID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Images", "PID", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "imageId");
        }
    }
}
