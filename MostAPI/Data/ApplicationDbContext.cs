using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MostAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Admin> admins { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
