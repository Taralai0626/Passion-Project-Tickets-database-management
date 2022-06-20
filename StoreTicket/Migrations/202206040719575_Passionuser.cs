namespace StoreTicket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Passionuser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PassionUsers",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserFirstName = c.String(),
                        UserLastName = c.String(),
                        UserName = c.String(),
                        PhoneNnumber = c.String(),
                        EmailAddress = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PassionUsers");
        }
    }
}
