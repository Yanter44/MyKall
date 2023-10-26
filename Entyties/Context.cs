using Microsoft.EntityFrameworkCore;
using System;

namespace LerningAsp.Entyties
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public Context()
        {
          // Database.EnsureCreated();
           Database.OpenConnection();
            //Database.EnsureDeleted();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-S9AIDDH\\SQLEXPRESS; Database=AspUsers; Trusted_Connection=True; TrustServerCertificate=True");
        }
    }
}
