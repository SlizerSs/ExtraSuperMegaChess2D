namespace ChessAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLastMoveMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "LastMove", c => c.String(maxLength: 6));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "LastMove");
        }
    }
}
