using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;
using System;

namespace ExpenseTracker.Services
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Expense> Expenses { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Budget> Budgets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ExpenseTracker",
                "expenses.db"
            );

            var directory = System.IO.Path.GetDirectoryName(dbPath);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory!);
            }

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.PaymentMethod).HasMaxLength(50);
                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Expenses)
                      .HasForeignKey(e => e.CategoryId);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).HasMaxLength(100).IsRequired();
                entity.Property(c => c.Icon).HasMaxLength(10);
                entity.Property(c => c.Color).HasMaxLength(20);
            });

            modelBuilder.Entity<Budget>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.MonthlyLimit).HasColumnType("decimal(18,2)");
                entity.HasOne(b => b.Category)
                      .WithMany()
                      .HasForeignKey(b => b.CategoryId);
            });

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Food & Dining", Icon = "🍔", Color = "#FF5722", IsDefault = true },
                new Category { Id = 2, Name = "Transportation", Icon = "🚗", Color = "#2196F3", IsDefault = true },
                new Category { Id = 3, Name = "Shopping", Icon = "🛍️", Color = "#9C27B0", IsDefault = true },
                new Category { Id = 4, Name = "Entertainment", Icon = "🎮", Color = "#E91E63", IsDefault = true },
                new Category { Id = 5, Name = "Bills & Utilities", Icon = "💡", Color = "#FFC107", IsDefault = true },
                new Category { Id = 6, Name = "Healthcare", Icon = "⚕️", Color = "#00BCD4", IsDefault = true },
                new Category { Id = 7, Name = "Education", Icon = "📚", Color = "#673AB7", IsDefault = true },
                new Category { Id = 8, Name = "Others", Icon = "💰", Color = "#607D8B", IsDefault = true }
            );
        }
    }
}
