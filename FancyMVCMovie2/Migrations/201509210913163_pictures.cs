namespace FancyMVCMovie2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pictures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PictureModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageFull = c.Binary(),
                        ImageThumbnail = c.Binary(),
                        MovieId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Movies", t => t.MovieId, cascadeDelete: true)
                .Index(t => t.MovieId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PictureModels", "MovieId", "dbo.Movies");
            DropIndex("dbo.PictureModels", new[] { "MovieId" });
            DropTable("dbo.PictureModels");
        }
    }
}
