namespace WEbAPiIdentity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Products");
            AlterColumn("dbo.Products", "PID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Products", "PID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Products");
            AlterColumn("dbo.Products", "PID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Products", "PID");
        }
    }
}
