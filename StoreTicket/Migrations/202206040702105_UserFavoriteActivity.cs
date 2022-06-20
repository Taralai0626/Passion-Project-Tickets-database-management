namespace StoreTicket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserFavoriteActivity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FavoriteActivity", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "FavoriteActivity");
        }
    }
}
