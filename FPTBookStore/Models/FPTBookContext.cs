using System;
using System.Collections.Generic;
using FPTBookStore.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using FPTBookStore.Models;

namespace FPTBookStore.Models
{
    public partial class FPTBookContext : IdentityDbContext<ApplicationUser>
    {
        public FPTBookContext(DbContextOptions<FPTBookContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-NRR5H9E;Database=FPTBook;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("book");

                entity.Property(e => e.BookId).HasColumnName("bookId");

                entity.Property(e => e.BookAuthor)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("bookAuthor");

                entity.Property(e => e.BookDescription)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("bookDescription");

                entity.Property(e => e.BookName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("bookName");

                entity.Property(e => e.BookPrice)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("bookPrice");

                entity.Property(e => e.BookImage)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("bookImage");
                entity.Property(e => e.GenreId).HasColumnName("genreId");
                entity.Property(e => e.BookSells).HasColumnName("bookSells");
                entity.Property(e => e.BookStock).HasColumnName("bookStock");
            });
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("order");

                entity.Property(e => e.OrderId).HasColumnName("orderId");
                entity.Property(e => e.UserName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("userName");
                entity.Property(e => e.OrderAddress)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("orderAddress");

                entity.Property(e => e.OrderPhone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("orderPhone");

                entity.Property(e => e.OrderDate)
                    .HasColumnName("orderDate");
                entity.Property(e => e.OrderStatus)
                    .HasColumnName("orderStatus");
                entity.Property(e => e.OrderTotal)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("orderTotal");
            });
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("order_book");
                entity.Property(e => e.OrderId).HasColumnName("orderId");
                entity.Property(e => e.BookId).HasColumnName("bookId");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
            });
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genre");

                entity.Property(e => e.GenreId).HasColumnName("genreId");

                entity.Property(e => e.GenreName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("genreName");
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("role");

                entity.Property(e => e.RoleId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("roleId");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("roleName");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("user");

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("fullName");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("userId");

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("user_role");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.UserId).HasColumnName("userId");
            });
            base.OnModelCreating(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        public DbSet<FPTBookStore.Models.Genre> Genre { get; set; }
        public DbSet<FPTBookStore.Models.Order> Order { get; set; }
        public DbSet<FPTBookStore.Models.OrderItem> OrderItem { get; set; }
    }
}
