namespace ChessAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseRefactoring : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PlayerStatistics", "PlayerID", "dbo.Players");
            DropIndex("dbo.PlayerStatistics", new[] { "PlayerID" });
            AddColumn("dbo.Players", "Games", c => c.Int());
            AddColumn("dbo.Players", "Wins", c => c.Int());
            AddColumn("dbo.Players", "Loses", c => c.Int());
            DropColumn("dbo.Sides", "OffersDraw");
            DropTable("dbo.PlayerStatistics");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PlayerStatistics",
                c => new
                    {
                        PlayerID = c.Int(nullable: false),
                        Games = c.Int(),
                        Wins = c.Int(),
                        Loses = c.Int(),
                        Draws = c.Int(),
                        Resigns = c.Int(),
                    })
                .PrimaryKey(t => t.PlayerID);
            
            AddColumn("dbo.Sides", "OffersDraw", c => c.Boolean(nullable: false));
            DropColumn("dbo.Players", "Loses");
            DropColumn("dbo.Players", "Wins");
            DropColumn("dbo.Players", "Games");
            CreateIndex("dbo.PlayerStatistics", "PlayerID");
            AddForeignKey("dbo.PlayerStatistics", "PlayerID", "dbo.Players", "PlayerID");
        }
    }
}
