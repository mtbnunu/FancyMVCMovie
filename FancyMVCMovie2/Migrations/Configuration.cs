using FancyMVCMovie2.Models;

namespace FancyMVCMovie2.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FancyMVCMovie2.Models.Context.DefaultContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FancyMVCMovie2.Models.Context.DefaultContext context)
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

        }
    }
}
