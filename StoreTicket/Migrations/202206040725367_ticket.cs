namespace StoreTicket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ticket : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        TicketId = c.Int(nullable: false, identity: true),
                        WebsiteId = c.Int(nullable: false),
                        EventName = c.String(),
                        EventType = c.String(),
                        EventVenue = c.String(),
                        EventLocation = c.String(),
                        EventDate = c.DateTime(nullable: false),
                        TicketStartingPrice = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.TicketId)
                .ForeignKey("dbo.Websites", t => t.WebsiteId, cascadeDelete: true)
                .Index(t => t.WebsiteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "WebsiteId", "dbo.Websites");
            DropIndex("dbo.Tickets", new[] { "WebsiteId" });
            DropTable("dbo.Tickets");
        }
    }
}
