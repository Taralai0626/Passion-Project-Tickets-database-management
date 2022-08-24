namespace StoreTicket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketHasPicTicket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "TicketHasPic", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "TicketHasPic");
        }
    }
}
