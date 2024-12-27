using Microsoft.EntityFrameworkCore;
using MostAPI.Data;
using System.Collections.Generic;

namespace MostAPI.Context
{
    public class PostgresDbContext : DbContext
    {
        public DbSet<Admin> admins { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Admin>().Property(a => a.ChatId).HasColumnName("chatid");
        }


        public PostgresDbContext(DbContextOptions<PostgresDbContext> options)
            : base(options)
        {
        }
    }
}
