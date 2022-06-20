namespace StoreTicket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Users", newName: "Passionusers");
            RenameTable(name: "dbo.TicketUsers", newName: "TicketPassionusers");
            RenameColumn(table: "dbo.TicketPassionusers", name: "User_UserId", newName: "Passionuser_UserId");
            RenameIndex(table: "dbo.TicketPassionusers", name: "IX_User_UserId", newName: "IX_Passionuser_UserId");
            AddColumn("dbo.Passionusers", "AccountName", c => c.String());
            DropColumn("dbo.Passionusers", "UserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Passionusers", "UserName", c => c.String());
            DropColumn("dbo.Passionusers", "AccountName");
            RenameIndex(table: "dbo.TicketPassionusers", name: "IX_Passionuser_UserId", newName: "IX_User_UserId");
            RenameColumn(table: "dbo.TicketPassionusers", name: "Passionuser_UserId", newName: "User_UserId");
            RenameTable(name: "dbo.TicketPassionusers", newName: "TicketUsers");
            RenameTable(name: "dbo.Passionusers", newName: "Users");
        }
    }
}
