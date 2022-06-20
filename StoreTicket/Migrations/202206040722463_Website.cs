namespace StoreTicket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Website : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Websites",
                c => new
                    {
                        WebsiteId = c.Int(nullable: false, identity: true),
                        WebsiteName = c.String(),
                        WebsiteType = c.String(),
                    })
                .PrimaryKey(t => t.WebsiteId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Websites");
        }
    }
}
