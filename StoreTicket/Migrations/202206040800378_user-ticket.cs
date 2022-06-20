namespace StoreTicket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userticket : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TicketUsers",
                c => new
                    {
                        Ticket_TicketId = c.Int(nullable: false),
                        User_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ticket_TicketId, t.User_UserId })
                .ForeignKey("dbo.Tickets", t => t.Ticket_TicketId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserId, cascadeDelete: true)
                .Index(t => t.Ticket_TicketId)
                .Index(t => t.User_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketUsers", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.TicketUsers", "Ticket_TicketId", "dbo.Tickets");
            DropIndex("dbo.TicketUsers", new[] { "User_UserId" });
            DropIndex("dbo.TicketUsers", new[] { "Ticket_TicketId" });
            DropTable("dbo.TicketUsers");
        }
    }
}
