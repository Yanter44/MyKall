using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

namespace LerningAsp.Entyties
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> userProfile { get; set;}
        public Context()
        {
            Database.EnsureCreated();
            Database.OpenConnection();
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>()
          .HasOne(up => up.User)
          .WithOne(u => u.Profile)
          .HasForeignKey<UserProfile>(up => up.UserId);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-S9AIDDH\\SQLEXPRESS; Database=AspUsers; Trusted_Connection=True; TrustServerCertificate=True");
        }
       
    }



  
}
