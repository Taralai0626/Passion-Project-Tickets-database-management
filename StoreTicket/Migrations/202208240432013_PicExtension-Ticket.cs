namespace StoreTicket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PicExtensionTicket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "PicExtension", c => c.String());
            DropColumn("dbo.Tickets", "TicketPhoto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tickets", "TicketPhoto", c => c.String());
            DropColumn("dbo.Tickets", "PicExtension");
        }
    }
}
