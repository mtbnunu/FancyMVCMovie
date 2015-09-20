using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FancyMVCMovie.Models
{
    public class DefaultContext : DbContext
    {
        public DefaultContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<Genre> genres { get; set; }
        public DbSet<Movie> movies { get; set; }
    }
}