using System;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Entities;
using WebApplication5.Models;

namespace WebApplication5.Repositories
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

       
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tùy chỉnh bảng User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Age).IsRequired();
                entity.Property(u => u.Address).HasMaxLength(250);
                entity.Property(u => u.DateOfBirth).IsRequired();
                entity.Property(u => u.Ethnicity).HasMaxLength(50);
                entity.Property(u => u.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(u => u.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // Tùy chỉnh bảng Account
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Accounts");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Email).IsRequired().HasMaxLength(100);
                entity.Property(a => a.PasswordHash).IsRequired();
                entity.Property(a => a.IsVerified).HasDefaultValue(false);
                entity.Property(a => a.Otp).HasMaxLength(6);
                entity.Property(a => a.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(a => a.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

                // Quan hệ 1-1 với bảng User
                entity.HasOne(a => a.User)
                      .WithOne()
                      .HasForeignKey<Account>(a => a.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
