using FancyMVCMovie.Models;

namespace FancyMVCMovie.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FancyMVCMovie.Models.DefaultContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FancyMVCMovie.Models.DefaultContext context)
        {
            context.genres.AddOrUpdate(new Genre()
            {
                Id = 1,
                Name = "Comedy"
            });
            context.genres.AddOrUpdate(new Genre()
            {
                Id = 2,
                Name = "Action"
            });
            context.genres.AddOrUpdate(new Genre()
            {
                Id = 3,
                Name = "Horror"
            });
            context.genres.AddOrUpdate(new Genre()
            {
                Id = 4,
                Name = "Drama"
            });
            context.genres.AddOrUpdate(new Genre()
            {
                Id = 5,
                Name = "Sci-Fi"
            });


            context.SaveChanges();
        }
    }
}
