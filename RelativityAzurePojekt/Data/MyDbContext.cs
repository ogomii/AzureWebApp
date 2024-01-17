using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RelativityAzurePojekt.Models
{
    public class MyDatabaseContext : DbContext
    {
        public MyDatabaseContext(DbContextOptions<MyDatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<RelativityAzurePojekt.Models.Movie> Movie { get; set; }
        public DbSet<RelativityAzurePojekt.Models.Review> Review { get; set; }
        public DbSet<RelativityAzurePojekt.Models.AppUser> AppUser { get; set; }
    }
}