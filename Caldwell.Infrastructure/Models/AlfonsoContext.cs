using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Caldwell.Infrastructure.Models
{
    public partial class AlfonsoContext : DbContext
    {
        public AlfonsoContext()
        {
        }

        public AlfonsoContext(DbContextOptions<AlfonsoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Catalog> Catalog { get; set; }
        public virtual DbSet<CatalogBrand> CatalogBrand { get; set; }
        public virtual DbSet<CatalogType> CatalogType { get; set; }
        public virtual DbSet<CompareItem> CompareItem { get; set; }
        public virtual DbSet<Compares> Compares { get; set; }
        public virtual DbSet<FeatureItem> FeatureItem { get; set; }
        public virtual DbSet<Features> Features { get; set; }
        public virtual DbSet<WishlistItem> WishlistItem { get; set; }
        public virtual DbSet<Wishlists> Wishlists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Alfonso;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<Catalog>(entity =>
            {                
                entity.HasIndex(e => e.CatalogBrandId);

                entity.HasIndex(e => e.CatalogTypeId);

                entity.Property(e => e.Id).ForSqlServerUseSequenceHiLo("catalog_hilo").IsRequired(); // .ValueGeneratedNever();

                entity.Property(e => e.Cpu).HasColumnName("CPU");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.CatalogBrand)
                    .WithMany(p => p.Catalog)
                    .HasForeignKey(d => d.CatalogBrandId);

                entity.HasOne(d => d.CatalogType)
                    .WithMany(p => p.Catalog)
                    .HasForeignKey(d => d.CatalogTypeId);
            });

            modelBuilder.Entity<CatalogBrand>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Brand)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<CatalogType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<CompareItem>(entity =>
            {
                entity.HasIndex(e => e.CompareId);

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Compare)
                    .WithMany(p => p.CompareItem)
                    .HasForeignKey(d => d.CompareId);
            });

            modelBuilder.Entity<FeatureItem>(entity =>
            {
                entity.HasIndex(e => e.FeatureId);

                entity.HasOne(d => d.Feature)
                    .WithMany(p => p.FeatureItem)
                    .HasForeignKey(d => d.FeatureId);
            });

            modelBuilder.Entity<Features>(entity =>
            {
                entity.HasIndex(e => e.CatalogItemId);

                entity.HasOne(d => d.CatalogItem)
                    .WithMany(p => p.Features)
                    .HasForeignKey(d => d.CatalogItemId);
            });

            modelBuilder.Entity<WishlistItem>(entity =>
            {
                entity.HasIndex(e => e.WishlistId);

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Wishlist)
                    .WithMany(p => p.WishlistItem)
                    .HasForeignKey(d => d.WishlistId);
            });

            modelBuilder.HasSequence("catalog_brand_hilo").IncrementsBy(10);

            modelBuilder.HasSequence("catalog_hilo").IncrementsBy(10);

            modelBuilder.HasSequence("catalog_type_hilo").IncrementsBy(10);
        }
    }
}
