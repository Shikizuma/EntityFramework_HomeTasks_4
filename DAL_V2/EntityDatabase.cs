using DAL_V2.Entity;
using DAL_V2.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_V2
{
    public class EntityDatabase : DbContext
    {
        public DbSet<Product> Product { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<KeyParams> KeyLink { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Word> Words { get; set; }

        public EntityDatabase()
        {
            Database.EnsureCreated();   
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS; Database=HomeTaskThird; Trusted_Connection=True; TrustServerCertificate=True");
        }
    }
}
