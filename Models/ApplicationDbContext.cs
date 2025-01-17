using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace POS1.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Users> Users { get; set; }
        public DbSet<Profile> UserProfiles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Transaction> Transactions { get; set; }                        
        public DbSet<TransactionItem> TransactionItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}