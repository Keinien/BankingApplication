using BankAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Data;

public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options) { }

        public DbSet<BankAccount> Accounts => Set<BankAccount>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>()
                .HasIndex(x => x.AccountNumber)
                .IsUnique();
            modelBuilder.Entity<Transaction>()
                .Property(x => x.Type)
                .HasConversion<string>();
        }
    }

