namespace StoreTicket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ticketphoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "TicketPhoto", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "TicketPhoto");
        }
    }
}
