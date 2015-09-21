namespace FancyMVCMovie2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class id_noreq : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movies", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movies", "UserId", c => c.String(nullable: false));
        }
    }
}
