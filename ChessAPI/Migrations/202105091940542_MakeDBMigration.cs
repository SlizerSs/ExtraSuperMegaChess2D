namespace ChessAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeDBMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        GameID = c.Int(nullable: false, identity: true),
                        FEN = c.String(maxLength: 255, unicode: false),
                        Status = c.String(maxLength: 4, unicode: false),
                        Winner = c.String(maxLength: 63),
                    })
                .PrimaryKey(t => t.GameID);
            
            CreateTable(
                "dbo.Sides",
                c => new
                    {
                        SideID = c.Int(nullable: false, identity: true),
                        GameID = c.Int(),
                        PlayerID = c.Int(),
                        Color = c.String(maxLength: 1, unicode: false),
                        OffersDraw = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SideID)
                .ForeignKey("dbo.Games", t => t.GameID)
                .ForeignKey("dbo.Players", t => t.PlayerID)
                .Index(t => t.GameID)
                .Index(t => t.PlayerID);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        PlayerID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30, unicode: false),
                        Password = c.String(maxLength: 255, unicode: false),
                    })
                .PrimaryKey(t => t.PlayerID);
            
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
                .PrimaryKey(t => t.PlayerID)
                .ForeignKey("dbo.Players", t => t.PlayerID)
                .Index(t => t.PlayerID);
            
            CreateTable(
                "dbo.sysdiagrams",
                c => new
                    {
                        diagram_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 128),
                        principal_id = c.Int(nullable: false),
                        version = c.Int(),
                        definition = c.Binary(),
                    })
                .PrimaryKey(t => t.diagram_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sides", "PlayerID", "dbo.Players");
            DropForeignKey("dbo.PlayerStatistics", "PlayerID", "dbo.Players");
            DropForeignKey("dbo.Sides", "GameID", "dbo.Games");
            DropIndex("dbo.PlayerStatistics", new[] { "PlayerID" });
            DropIndex("dbo.Sides", new[] { "PlayerID" });
            DropIndex("dbo.Sides", new[] { "GameID" });
            DropTable("dbo.sysdiagrams");
            DropTable("dbo.PlayerStatistics");
            DropTable("dbo.Players");
            DropTable("dbo.Sides");
            DropTable("dbo.Games");
        }
    }
}
