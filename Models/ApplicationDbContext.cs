using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace POS1.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        //public required DbSet<Product> Products { get; set; }
        public required DbSet<Users> Users { get; set; }

        public required DbSet<Profile> UserProfiles { get; set; }
     //   public required DbSet<ProductImage> ProductImages { get; set; }
    }
}