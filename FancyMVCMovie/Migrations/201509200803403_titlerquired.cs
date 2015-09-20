namespace FancyMVCMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class titlerquired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movies", "MovieTitle", c => c.String(nullable: false, maxLength: 60));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movies", "MovieTitle", c => c.String(maxLength: 60));
        }
    }
}
