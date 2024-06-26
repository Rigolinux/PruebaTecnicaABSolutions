﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PruebaTecnicaABSolutions.Models
{
    public partial class ABPruebaTecnicaContext : DbContext
    {
        public ABPruebaTecnicaContext()
        {
        }

        public ABPruebaTecnicaContext(DbContextOptions<ABPruebaTecnicaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Business> Businesses { get; set; } = null!;
        public virtual DbSet<MenuCategory> MenuCategories { get; set; } = null!;
        public virtual DbSet<MenuItem> MenuItems { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserType> UserTypes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
              optionsBuilder.UseSqlServer("Server=.\\RIGO;Database=ABPruebaTecnica;encrypt=True;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Business>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.BusinessName).HasMaxLength(100);

                entity.Property(e => e.CreationDate).HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.Phone).HasMaxLength(20);
            });

            modelBuilder.Entity<MenuCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK__MenuCate__19093A0BD6EA2008");

                entity.Property(e => e.CategoryName).HasMaxLength(50);

                entity.HasOne(d => d.Business)
                    .WithMany(p => p.MenuCategories)
                    .HasForeignKey(d => d.BusinessId)
                    .HasConstraintName("FK__MenuCateg__Busin__412EB0B6");
            });

            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.HasKey(e => e.ItemId)
                    .HasName("PK__MenuItem__727E838B0F6AACD8");

                entity.Property(e => e.AddedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ItemName).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Business)
                    .WithMany(p => p.MenuItems)
                    .HasForeignKey(d => d.BusinessId)
                    .HasConstraintName("FK__MenuItems__Busin__45F365D3");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.MenuItems)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__MenuItems__Categ__44FF419A");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UC_Email")
                    .IsUnique();

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.HasOne(d => d.Business)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.BusinessId)
                    .HasConstraintName("FK__Users__BusinessI__32E0915F");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserTypeId)
                    .HasConstraintName("FK__Users__UserTypeI__31EC6D26");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.Property(e => e.TypeName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
