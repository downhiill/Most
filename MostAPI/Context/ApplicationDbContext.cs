using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MostAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Admin> admins { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Admin>().Property(a => a.ChatId).HasColumnName("chatid");
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
